using k8s;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimecheckController.CustomResources;

namespace TimecheckController.Handlers
{
    class TimecheckDeletedHandler
    {
        private readonly Kubernetes _kubernetes;

        public TimecheckDeletedHandler(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task HandleAsync(Timecheck timecheck)
        {
            await DeleteDeploymentAsync(timecheck);
        }

        private async Task DeleteDeploymentAsync(Timecheck timecheck)
        {
            var name = timecheck.Metadata.Name;
            var deployments = await _kubernetes.ListNamespacedDeploymentAsync(
                                                namespaceParameter: Program.NamespaceName,
                                                fieldSelector: $"metadata.name={name}");

            if (deployments.Items.Any())
            {
                await _kubernetes.DeleteNamespacedDeploymentAsync(name, Program.NamespaceName);
                Console.WriteLine($"** Deleted Deployment: {name}, in namespace: {Program.NamespaceName}");
            }
            else
            {
                Console.WriteLine($"** No Deployment to delete: {name}, in namespace: {Program.NamespaceName}");
            }
        }
    }
}
