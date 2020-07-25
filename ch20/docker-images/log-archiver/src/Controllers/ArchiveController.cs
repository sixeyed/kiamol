using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using io = System.IO;

namespace LogArchiver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArchiveController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ArchiveController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var path = _config["Archive:LogFile"];
            if (!io.File.Exists(path))
            {
                return NotFound();
            }

            var logs = io.File.ReadAllText(path); 
            Console.WriteLine($"Read logs from: {path}");

            try
            {
                //io.File.Delete(path);     
                //Console.WriteLine($"Deleted log file: {path}");

                io.File.WriteAllText(path, string.Empty);
                Console.WriteLine($"Created empty log file: {path}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to clear log file: {path}; ex: {ex}");
            }

            return Ok(logs);
        }
    }
}
