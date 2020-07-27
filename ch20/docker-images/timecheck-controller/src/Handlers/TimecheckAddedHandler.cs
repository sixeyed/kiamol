using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimecheckController.CustomResources;

namespace TimecheckController.Handlers
{
    public class TimecheckAddedHandler
    {
        private readonly Kubernetes _kubernetes;

        public TimecheckAddedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task HandleAsync(Timecheck timecheck)
        {
            await EnsureDeploymentAsync(timecheck);
        }

        private async Task EnsureDeploymentAsync(Timecheck timecheck)
        {
            var name = timecheck.Metadata.Name;
            var deployments = await _kubernetes.ListNamespacedDeploymentAsync(
                                                namespaceParameter: Program.NamespaceName,
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
                                { "app", "timecheck"},
                                { "instance", name }
                            }
                        },
                        Template = new V1PodTemplateSpec
                        {
                            Metadata = new V1ObjectMeta
                            {
                                Labels = new Dictionary<string, string>()
                                {
                                    { "app", "timecheck"},
                                    { "instance", name }
                                }
                            },
                            Spec = new V1PodSpec
                            {
                                AutomountServiceAccountToken = false,
                                Containers = new List<V1Container>
                                {
                                    new V1Container
                                    {
                                        Name = "tc",
                                        Image = "kiamol/ch07-timecheck",
                                        Env = new List<V1EnvVar>
                                        {
                                            new V1EnvVar
                                            {
                                                Name = "Application__Environment",
                                                Value = timecheck.Spec.Environment
                                            },
                                            new V1EnvVar
                                            {
                                                Name = "Timer__IntervalSeconds",
                                                Value = timecheck.Spec.Interval.ToString()
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
                                    },
                                    new V1Container
                                    {
                                        Name = "logger",
                                        Image = "kiamol/ch03-sleep",
                                        Command = new List<string>
                                        {
                                            "sh", "-c", "tail -f /logs-ro/timecheck.log"
                                        },
                                        VolumeMounts = new List<V1VolumeMount>
                                        {
                                            new V1VolumeMount
                                            {
                                                Name= "logs",
                                                MountPath = "/logs-ro",
                                                ReadOnlyProperty = true
                                            }
                                        }

                                    }
                                },
                                Volumes = new List<V1Volume>
                                {
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
            }
            else
            {
                Console.WriteLine($"** Deployment exists: {name}, in namespace: {Program.NamespaceName}");
            }
        }
    }
}
