using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ToDoList.Pages
{
    public class ConfigModel : PageModel
    {
        private readonly ILogger<ConfigModel> _logger;
        private readonly IConfiguration _config;

        public IEnumerable<KeyValuePair<string, string>> ConfigItems { get; private set; }

        public ConfigModel(IConfiguration config, ILogger<ConfigModel> logger)
        {
            _config = config;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (_config.GetValue<bool>("ConfigController:Enabled"))
            {
                _logger.LogDebug("GET /config called");
                ConfigItems = _config.AsEnumerable();
                return Page();
            }
            else
            {
                _logger.LogWarning("Attempt to view config settings");
                return NotFound();
            }            
        }
    }
}