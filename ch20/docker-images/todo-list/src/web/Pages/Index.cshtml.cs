using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoList.Services;

namespace ToDoList.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly ILogger<IndexModel> _logger;
        private readonly ToDoService _todoService;

        public int TodoCount { get; private set; }

        public IndexModel(ToDoService todoService, IConfiguration config, ILogger<IndexModel> logger)
        {
            _todoService = todoService;
            _config = config;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogDebug("GET / called");
            
            if (!string.IsNullOrEmpty(_config["BannerMessage"]))
            {
                ViewData["BannerMessage"] = _config["BannerMessage"];
            }

            TodoCount = await _todoService.GetToDoCountAsync();            
            _logger.LogDebug($"Fetched count: {TodoCount} from service");

            return Page();
        }
    }
}