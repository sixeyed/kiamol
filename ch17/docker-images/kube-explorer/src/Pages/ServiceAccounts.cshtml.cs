using k8s;
using KubeExplorer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KubeExplorer.Pages
{
    public class ServiceAccountsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Kubernetes _kubernetes;

        public string Namespace { get; set; }

        public IEnumerable<ServiceAccountModel> ServiceAccounts { get; set; }

        public ServiceAccountsModel(Kubernetes kubernetes, ILogger<IndexModel> logger)
        {
            _kubernetes = kubernetes;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string ns="default")
        {
            Namespace = ns;
            _logger.LogDebug($"GET / called for ns: {Namespace}");

            var sas = await _kubernetes.ListNamespacedServiceAccountAsync(Namespace);
            ServiceAccounts = sas.Items.Select(x => new ServiceAccountModel
            {
                Name = x.Metadata.Name,
                AutomountServiceAccountToken = x.AutomountServiceAccountToken ?? true,
                SecretCount = x.Secrets.Count()
            }) ;

            _logger.LogDebug($"Fetched {ServiceAccounts.Count()} ServiceAccounts.");
            return Page();
        }
    }
}
