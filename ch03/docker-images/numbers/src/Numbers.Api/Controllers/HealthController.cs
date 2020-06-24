using Microsoft.AspNetCore.Mvc;

namespace Numbers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Route("healthz")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            if (Status.Healthy)
            {                
                return Ok("Ok");
            }
            else
            {
                return StatusCode(500, new { message = "Unhealthy" });
            }
        }
    }
}
