using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;

namespace WebPingOperator.WebPingerController.Handlers
{
    public class WebPingerAddedHandler
    {
        private readonly Kubernetes _kubernetes;

        public WebPingerAddedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task<bool> HandleAsync(WebPinger pinger)
        {
            await EnsureConfigMapAsync(pinger);
            await EnsureDeploymentAsync(pinger);
            await EnsureServiceAsync(pinger);

            return true;
        }

        private async Task<bool> EnsureConfigMapAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}-config";
            var configMaps = await _kubernetes.ListNamespacedConfigMapAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (!configMaps.Items.Any())
            {
                var configMap = new V1ConfigMap
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = name,
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20" },
                        }
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "logConfig.js", ConfigMapData.logConfig.Replace("\r\n", "\n") }
                    }
                };

                await _kubernetes.CreateNamespacedConfigMapAsync(configMap, Program.NamespaceName);
                Console.WriteLine($"** Created ConfigMap: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** ConfigMap exists: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }

        private async Task<bool> EnsureDeploymentAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}";
            var deployments = await _kubernetes.ListNamespacedDeploymentAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (!deployments.Items.Any())
            {
                var deployment = new V1Deployment
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = name,
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20" },
                        }
                    },
                    Spec = new V1DeploymentSpec
                    {
                        Selector = new V1LabelSelector
                        {
                            MatchLabels = new Dictionary<string, string>()
                            {
                                { "app", "web-ping"},
                                { "instance", name }
                            }
                        },
                        Template = new V1PodTemplateSpec
                        {
                            Metadata = new V1ObjectMeta
                            {
                                Labels = new Dictionary<string, string>()
                                {
                                    { "app", "web-ping"},
                                    { "instance", name },
                                    { "target", pinger.Spec.Target }
                                }
                            },
                            Spec = new V1PodSpec
                            {
                                AutomountServiceAccountToken = false,
                                Containers = new List<V1Container>
                                {
                                        new V1Container
                                        {
                                            Name = "web",
                                            Image = "kiamol/ch10-web-ping",
                                            Env = new List<V1EnvVar>
                                            {
                                                new V1EnvVar
                                                {
                                                    Name = "INTERVAL",
                                                    Value = pinger.Spec.GetIntervalMilliseconds().ToString()
                                                },
                                                new V1EnvVar
                                                {
                                                    Name = "TARGET",
                                                    Value = pinger.Spec.Target
                                                },
                                                new V1EnvVar
                                                {
                                                    Name = "METHOD",
                                                    Value = pinger.Spec.GetMethod()
                                                }
                                            },
                                            VolumeMounts = new List<V1VolumeMount>
                                            {
                                                new V1VolumeMount
                                                {
                                                    Name= "config",
                                                    MountPath = "/app/config",
                                                    ReadOnlyProperty = true
                                                },
                                                new V1VolumeMount
                                                {
                                                    Name= "logs",
                                                    MountPath = "/logs"
                                                }
                                            }
                                        },
                                    new V1Container
                                        {
                                        Name = "api",
                                            Image = "kiamol/ch20-log-archiver",
                                            Ports = new List<V1ContainerPort>
                                            {
                                                new V1ContainerPort
                                                {
                                                    Name = "api",
                                                    ContainerPort = 80
                                                }
                                            },
                                            VolumeMounts = new List<V1VolumeMount>
                                            {
                                                new V1VolumeMount
                                                {
                                                    Name= "logs",
                                                    MountPath = "/logs"
                                                }
                                            }

                                        }
                                },
                                Volumes = new List<V1Volume>
                                { 
                                    new V1Volume
                                    {
                                        Name = "config",
                                        ConfigMap = new V1ConfigMapVolumeSource
                                        {
                                            Name = $"{name}-config"
                                        }
                                    },
                                    new V1Volume
                                    {
                                        Name = "logs",
                                        EmptyDir = new V1EmptyDirVolumeSource()
                                    }
                                }
                            }
                        }
                    }
                };

                await _kubernetes.CreateNamespacedDeploymentAsync(deployment, Program.NamespaceName);
                Console.WriteLine($"** Created Deployment: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** Deployment exists: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }

        private async Task<bool> EnsureServiceAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}";
            var services = await _kubernetes.ListNamespacedServiceAsync(
                                             Program.NamespaceName,
                                             fieldSelector: $"metadata.name={name}");

            if (!services.Items.Any())
            {
                var service = new V1Service
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = name,
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20"},
                            { "app", "web-ping"},
                            { "target", pinger.Spec.Target }
                        }
                    },
                    Spec = new V1ServiceSpec
                    {
                        Type = "ClusterIP",
                        Ports = new List<V1ServicePort>()
                        {
                            new V1ServicePort
                            {
                                Port = 8080,
                                TargetPort = "api"
                            }
                        },
                        Selector = new Dictionary<string, string>()
                        {
                            { "app", "web-ping"},
                            { "instance", name }
                        }
                    }
                };

                await _kubernetes.CreateNamespacedServiceAsync(service, Program.NamespaceName);
                Console.WriteLine($"** Created Service: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** Service exists: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }
    }
}