using k8s;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebPingOperator.Installer.Installers;

namespace WebPingOperator.Installer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config/appsettings.json", optional: true)
                .Build();

            var kubeConfig = KubernetesClientConfiguration.IsInCluster() ?
                             KubernetesClientConfiguration.InClusterConfig() :
                             new KubernetesClientConfiguration { Host = "http://localhost:8001" };

            var kubernetes = new Kubernetes(kubeConfig);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(config)
                .AddSingleton(kubernetes)
                .AddTransient<IInstaller, WebPingerInstaller>()
                .AddTransient<IInstaller, WebPingerArchiveInstaller>()
                .BuildServiceProvider();

            var installers = serviceProvider.GetServices<IInstaller>();
            foreach (var installer in installers)
            {
                await installer.InstallAsync();
            }

            Console.WriteLine("* Done.");
        }
    }
}
