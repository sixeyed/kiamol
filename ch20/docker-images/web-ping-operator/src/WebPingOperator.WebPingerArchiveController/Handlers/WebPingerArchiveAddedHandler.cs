using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;

namespace WebPingOperator.WebPingerArchiveController.Handlers
{
    public class WebPingerArchiveAddedHandler
    {
        private readonly Kubernetes _kubernetes;

        public WebPingerArchiveAddedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task<bool> HandleAsync(WebPingerArchive archive)
        {
            await CreateJobAsync(archive);
            return true;
        }
        
        private async Task<bool> CreateJobAsync(WebPingerArchive archive)
        {
            // check a pinger exists to archive:
            var services = await _kubernetes.ListNamespacedServiceAsync(
                                             Program.NamespaceName,
                                             labelSelector: $"app=web-ping,target={archive.Spec.Target}");
            if (!services.Items.Any())
            {
                Console.WriteLine($"** No WebPinger Service exists for target: {archive.Spec.Target}, in namespace: {Program.NamespaceName}");
                return false;
            }
            
            var pingerServiceName = services.Items.First().Metadata.Name;
            var name = $"wpa-{archive.Metadata.Name}-{archive.Metadata.CreationTimestamp:yyMMdd-HHmm}";
            var jobs = await _kubernetes.ListNamespacedJobAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (!jobs.Items.Any())
            {
                var job = new V1Job
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = name,
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20" },
                        }
                    },
                    Spec = new V1JobSpec
                    {
                        Completions = 1,
                        Template = new V1PodTemplateSpec
                        {
                            Metadata = new V1ObjectMeta
                            {
                                Labels = new Dictionary<string, string>()
                                {
                                    { "app", "web-ping-archive"},
                                    { "target", archive.Spec.Target }
                                }
                            },
                            Spec = new V1PodSpec
                            {
                                AutomountServiceAccountToken = false,
                                RestartPolicy = "Never",
                                Containers = new List<V1Container>
                                {
                                    new V1Container
                                    {
                                        Name = "archiver",
                                        Image = "kiamol/ch20-web-ping-archiver",
                                        Env = new List<V1EnvVar>
                                        {
                                            new V1EnvVar
                                            {
                                                Name = "WEB_PING_URL",
                                                Value = $"http://{pingerServiceName}:8080/archive"
                                            }
                                        }
                                     }
                                }
                            }
                        }
                    }
                };

                await _kubernetes.CreateNamespacedJobAsync(job, Program.NamespaceName);
                Console.WriteLine($"** Created Job: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** Job exists: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }
    }
}