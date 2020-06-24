using Microsoft.AspNetCore.Mvc;

namespace Numbers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResetController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Status.Healthy = true;
            return Ok("Ok");
        }
    }
}
