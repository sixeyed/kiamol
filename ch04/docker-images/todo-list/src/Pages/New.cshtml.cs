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
    public class NewModel : PageModel
    {
        private readonly ILogger<NewModel> _logger;
        private readonly ToDoService _todoService;

        [BindProperty]
        public ToDo ToDo { get; set; }

        public NewModel(ToDoService todoService, ILogger<NewModel> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogDebug("POST /new called");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ToDo.DateAdded = DateTime.Now;
            await _todoService.AddToDoAsync(ToDo);
            _logger.LogInformation("New item created");

            return RedirectToPage("/list");
        }
    }
}