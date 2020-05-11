using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SimpleProxy
{
    class Program
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static IConfiguration _Config;

        static void Main(string[] args)
        {
            _Config = GetConfig();

            var port = _Config.GetValue<int>("Proxy:Port");
            var proxyServer = new ProxyServer();
            proxyServer.BeforeRequest += OnRequest;

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, port, false);
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            Console.WriteLine($"** Logging proxy listening on port: {port} **");
            _ResetEvent.WaitOne();

            proxyServer.BeforeRequest -= OnRequest;
            proxyServer.Stop();
        }

        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        public static Task OnRequest(object sender, SessionEventArgs e)
        {
            var sourceUri = e.HttpClient.Request.RequestUri.AbsoluteUri;
            if (sourceUri == _Config.GetValue<string>("Proxy:Request:UriMap:Source"))
            {
                var targetUri = _Config.GetValue<string>("Proxy:Request:UriMap:Target");
                Console.WriteLine($"{e.HttpClient.Request.Method} {sourceUri} -> {targetUri}");
                e.HttpClient.Request.RequestUri = new Uri(targetUri);
            }
            else if (_Config.GetValue<bool>("Proxy:Request:RejectUnknown"))
            {
                Console.WriteLine($"{e.HttpClient.Request.Method} {e.HttpClient.Request.Url} [BLOCKED]");
                e.Ok("");
            }
            else
            {
                Console.WriteLine($"{e.HttpClient.Request.Method} {e.HttpClient.Request.Url}");
            }
            return Task.CompletedTask;
        }
    }
}
