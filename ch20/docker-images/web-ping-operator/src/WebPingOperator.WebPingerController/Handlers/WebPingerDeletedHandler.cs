using k8s;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;

namespace WebPingOperator.WebPingerController.Handlers
{
    public class WebPingerDeletedHandler
    {
        private readonly Kubernetes _kubernetes;

        public WebPingerDeletedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task<bool> HandleAsync(WebPinger pinger)
        {
            await DeleteServiceAsync(pinger);
            await DeleteDeploymentAsync(pinger);
            await DeleteConfigMapAsync(pinger);

            return true;
        }

        private async Task<bool> DeleteServiceAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}";
            var services = await _kubernetes.ListNamespacedServiceAsync(
                                             Program.NamespaceName,
                                             fieldSelector: $"metadata.name={name}");

            if (services.Items.Any())
            {
                await _kubernetes.DeleteNamespacedServiceAsync(name, Program.NamespaceName);
                Console.WriteLine($"** Deleted Service: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** No Service to delete: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }

        private async Task<bool> DeleteDeploymentAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}";
            var deployments = await _kubernetes.ListNamespacedDeploymentAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (deployments.Items.Any())
            {
                await _kubernetes.DeleteNamespacedDeploymentAsync(name, Program.NamespaceName);
                Console.WriteLine($"** Deleted Deployment: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** No Deployment to delete: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }

        private async Task<bool> DeleteConfigMapAsync(WebPinger pinger)
        {
            var name = $"wp-{pinger.Metadata.Name}-config";
            var configMaps = await _kubernetes.ListNamespacedConfigMapAsync(
                                                Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (configMaps.Items.Any())
            {
                await _kubernetes.DeleteNamespacedConfigMapAsync(name, Program.NamespaceName);
                Console.WriteLine($"** Deleted ConfigMap: {name}, in namespace: {Program.NamespaceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"** No ConfigMap to delete: {name}, in namespace: {Program.NamespaceName}");
                return false;
            }
        }
    }
}
