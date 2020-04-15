using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ToDoList.Model;
using ToDoList.Services;

namespace ToDoList.Pages
{
    public class ListModel : PageModel
    {
        private readonly ILogger<ListModel> _logger;
        private readonly ToDoService _todoService;

        public IEnumerable<ToDo> ToDos { get; private set; }

        public ListModel(ToDoService todoService, ILogger<ListModel> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            _logger.LogDebug("GET /list called");
            ToDos = await _todoService.GetToDosAsync();
            _logger.LogDebug($"Fetched {ToDos.Count()} items from service");
            return Page();
        }
    }
}