using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ToDoList.Services;

namespace ToDoList.Pages
{
    public class DiagnosticsModel : PageModel
    {
        private readonly ILogger<DiagnosticsModel> _logger;
        private readonly DiagnosticsService _diagnosticsService;

        public Model.Diagnostics Diagnostics { get; private set; }

        public DiagnosticsModel(DiagnosticsService diagnosticsService, ILogger<DiagnosticsModel> logger)
        {
            _diagnosticsService = diagnosticsService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Diagnostics = _diagnosticsService.GetDiagnostics();
            return Page();
        }
    }
}