using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToDoList.Model;
using ToDoList.Services;

namespace ToDoList.Pages
{
    public class NewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly ILogger<NewModel> _logger;
        private readonly ToDoService _todoService;

        [BindProperty]
        public ToDo ToDo { get; set; }

        public bool ReadOnly { get; set; }

        public NewModel(ToDoService todoService, IConfiguration config, ILogger<NewModel> logger)
        {
            _todoService = todoService;
            _config = config;
            _logger = logger;

            ReadOnly = _config.GetValue<bool>("Database:ReadOnly");            
        }

        public IActionResult OnGet()
        {
            if (ReadOnly)
            {
                ViewData["BannerMessage"] = "*** READ ONLY MODE ***";
            }
            return Page();
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

            return LocalRedirect("/list");
        }
    }
}