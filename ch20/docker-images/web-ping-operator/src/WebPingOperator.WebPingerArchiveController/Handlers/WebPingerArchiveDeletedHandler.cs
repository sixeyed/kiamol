using k8s;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;

namespace WebPingOperator.WebPingerArchiveController.Handlers
{
    public class WebPingerArchiveDeletedHandler
    {
        private readonly Kubernetes _kubernetes;

        public WebPingerArchiveDeletedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task<bool> HandleAsync(WebPingerArchive archive)
        {
            await DeleteJobAsync(archive);
            return true;
        }

        private async Task<bool> DeleteJobAsync(WebPingerArchive archive)
        {
            var name = $"wpa-{archive.Metadata.Name}-{archive.Metadata.CreationTimestamp:yyMMdd-HHmm}";
            var jobs = await _kubernetes.ListNamespacedJobAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (jobs.Items.Any())
            {
                await _kubernetes.DeleteNamespacedJobAsync(name, Program.NamespaceName);
                Console.WriteLine($"** Deleted Job: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** No Job to delete: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }
    }
}
