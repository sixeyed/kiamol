using k8s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace KubeExplorer.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Kubernetes _kubernetes;

        public string Namespace { get; set; }
        public string Pod { get; set; }

        public DeleteModel(Kubernetes kubernetes, ILogger<IndexModel> logger)
        {
            _kubernetes = kubernetes;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string name, string ns)
        {
            Namespace = ns;
            Pod = name;

            _logger.LogDebug($"POST /delete called for ns: {ns}; Pod: {name}");
            await _kubernetes.DeleteNamespacedPodAsync(name, ns);
            _logger.LogDebug($"Deleted Pod: {name}");

            
            return Page();
        }
    }
}