using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Numbers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Route("/sixeyed/kiamol/master/ch03/numbers/rng")] //this route matches the GitHub path for a static response
    public class RngController : ControllerBase
    {
        private static Random _Random = new Random();
        private static int _CallCount;

        private readonly ILogger<RngController> _logger;

        public RngController(ILogger<RngController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var n = _Random.Next(0,100);
            _logger.LogDebug($"Returning random number: {n}");
            return Ok(n);
        }
    }
}
