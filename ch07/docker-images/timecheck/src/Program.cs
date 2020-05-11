using System;
using System.Threading;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Kiamol.Ch07.TimeCheck
{
    class Program
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static string _Version;
        private static string _Env;

        public static void Main()
        {
            var config = new ConfigurationBuilder()
                             .AddJsonFile("/config/appsettings.json")
                             .Build();

            _Version = config["Application:Version"];
            _Env = config["Application:Environment"];
            var intervalSeconds = int.Parse(config["Timer:IntervalSeconds"]) * 1000;

            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .WriteTo.File("/logs/timecheck.log", shared: true, flushToDiskInterval: TimeSpan.FromSeconds(intervalSeconds))
                                .CreateLogger();

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
        }
    }
}
