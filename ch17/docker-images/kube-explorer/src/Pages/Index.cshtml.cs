using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using KubeExplorer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace KubeExplorer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Kubernetes _kubernetes;

        public string Namespace { get; set; }

        public IEnumerable<PodModel> Pods { get; set; }

        public IndexModel(Kubernetes kubernetes, ILogger<IndexModel> logger)
        {
            _kubernetes = kubernetes;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string ns="default")
        {
            Namespace = ns;
            _logger.LogDebug($"GET / called for ns: {Namespace}");

            var pods = await _kubernetes.ListNamespacedPodAsync(Namespace);
            Pods = pods.Items.Select(x => new PodModel
            {
                Name = x.Metadata.Name,
                Image = x.Spec.Containers[0].Image,
                Node = x.Spec.NodeName,
                Phase = x.Status.Phase
            }) ;

            _logger.LogDebug($"Fetched {Pods.Count()} pods.");
            return Page();
        }
    }
}
