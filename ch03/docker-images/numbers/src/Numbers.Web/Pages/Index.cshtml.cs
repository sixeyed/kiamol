using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Numbers.Web.Services;
using System;

namespace Numbers.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RandomNumberService _randomNumberService;

        public bool CallFailed { get; private set; }

        public int RandomNumber { get; private set; } = -1;

        public string ApiUrl 
        {
            get { return _randomNumberService.ApiUrl; }
        }

        public IndexModel(RandomNumberService randomNumberService, ILogger<IndexModel> logger)
        {
            _randomNumberService = randomNumberService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostAsync()
        {            
            try
            {
                RandomNumber = _randomNumberService.GetNumber();
                CallFailed = false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"API call failed: {ex}");
                RandomNumber = -1;
                CallFailed = true;
            }
            return Page();
        }
    }
}
