using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pi.Math;
using Pi.Web.Models;
using System;
using System.Diagnostics;

namespace Pi.Web.Controllers
{
    public class PiController : Controller
    {
        private readonly IConfiguration _config;
        private readonly bool _metricsEnabled;

        public PiController(IConfiguration config)
        {
            _config = config;
            _metricsEnabled = bool.Parse(_config["Computation:Metrics:Enabled"]);
        }

        public IActionResult Index(int? dp = 6)
        {
            var stopwatch = Stopwatch.StartNew();

            var pi = MachinFormula.Calculate(dp.Value, _metricsEnabled);

            var model = new PiViewModel
            {
                DecimalPlaces = dp.Value,
                Value = pi.ToString(),
                ComputeMilliseconds = stopwatch.ElapsedMilliseconds,
                ComputeHost = Environment.MachineName
            };

            return View(model);
        }
    }
}