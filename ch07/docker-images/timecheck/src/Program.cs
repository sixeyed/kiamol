using System;
using System.Threading;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;

namespace Kiamol.Ch07.TimeCheck
{
    class Program
    {
        private static readonly ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static  Counter _CheckCounter;
        private static string _Version;
        private static string _Env;

        public static void Main()
        {
            var config = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json")
                             .AddJsonFile("/config/appsettings.json", optional: true)
                             .AddEnvironmentVariables()
                             .Build();

            _Version = config["Application:Version"];
            _Env = config["Application:Environment"];
            var intervalSeconds = config.GetValue<int>("Timer:IntervalSeconds") * 1000;
            var metricsEnabled = config.GetValue<bool>("Metrics:Enabled");

            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .WriteTo.File("/logs/timecheck.log", shared: true, flushToDiskInterval: TimeSpan.FromSeconds(intervalSeconds))
                                .CreateLogger();

            if (metricsEnabled)
            {
                _CheckCounter = Metrics.CreateCounter("timecheck_total", "Number of timechecks");
                StartMetricServer(config);
            }

            using (var timer = new System.Timers.Timer(intervalSeconds))
            {
                timer.Elapsed += WriteTimeCheck;
                timer.Enabled = true;
                _ResetEvent.WaitOne();
            }
        }

        private static void WriteTimeCheck(Object source, ElapsedEventArgs e)
        {
            Log.Information("Environment: {environment}; version: {version}; time check: {timestamp}",
                                _Env, _Version, e.SignalTime.ToString("HH:mm.ss"));
            
            if (_CheckCounter != null)
            {
                _CheckCounter.Inc();
            }
        }

        private static void StartMetricServer(IConfiguration config)
        {
            var metricsPort = config.GetValue<int>("Metrics:Port");
            var server = new MetricServer(metricsPort);
            server.Start();
            Log.Information("Metrics server listening on port: {metricsPort}", metricsPort);
        }
    }
}
