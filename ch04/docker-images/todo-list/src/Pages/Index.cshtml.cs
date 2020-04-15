using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ToDoList.Services;

namespace ToDoList.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ToDoService _todoService;

        public int TodoCount { get; private set; }

        public IndexModel(ToDoService todoService, ILogger<IndexModel> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogDebug("GET / called");
            TodoCount = await _todoService.GetToDoCountAsync();
            _logger.LogDebug($"Fetched count: {TodoCount} from service");
            return Page();
        }
    }
}