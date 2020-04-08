using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Numbers.Web.Services;

namespace Numbers.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RandomNumberService _randomNumberService;

        public bool CallFailed { get; private set; }

        public int RandomNumber { get; private set; } = -1;

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
            catch
            {
                RandomNumber = -1;
                CallFailed = true;
            }
            return Page();
        }
    }
}
