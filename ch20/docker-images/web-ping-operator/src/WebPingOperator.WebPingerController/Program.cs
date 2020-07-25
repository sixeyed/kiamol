using k8s;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;
using WebPingOperator.WebPingerController.Handlers;

namespace WebPingOperator.WebPingerController
{
    class Program
    {
        private static ServiceProvider _ServiceProvider;

        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config/appsettings.json", optional: true)
                .Build();

            var kubeConfig = KubernetesClientConfiguration.IsInCluster() ?
                             KubernetesClientConfiguration.InClusterConfig() :
                             new KubernetesClientConfiguration { Host = "http://localhost:8001" };

            var kubernetes = new Kubernetes(kubeConfig);

            _ServiceProvider = new ServiceCollection()
                .AddSingleton(config)
                .AddSingleton(kubernetes)
                .AddTransient<WebPingerAddedHandler>()
                .AddTransient<WebPingerDeletedHandler>()
                .BuildServiceProvider();

            // TODO - this hangs and then fails if there are no objects
            var result = await kubernetes.ListNamespacedCustomObjectWithHttpMessagesAsync(
                            group: WebPinger.Definition.Group,
                            version: WebPinger.Definition.Version,
                            plural: WebPinger.Definition.Plural,
                            namespaceParameter: "default",
                            watch: true);

            using (result.Watch<WebPinger, object>(async (type, item) => await Handle(type, item)))
            {
                Console.WriteLine("* Watching for WebPinger events");
                
                var ctrlc = new ManualResetEventSlim(false);
                Console.CancelKeyPress += (sender, eventArgs) => ctrlc.Set();
                ctrlc.Wait();
            }         
        }

        public static async Task Handle(WatchEventType type, WebPinger pinger)
        {
            switch (type)
            {
                case WatchEventType.Added:
                    var addedhandler = _ServiceProvider.GetService<WebPingerAddedHandler>();
                    await addedhandler.HandleAsync(pinger);
                    Console.WriteLine($"* Handled event: {type}, for: {pinger.Metadata.Name}");
                    break;

                case WatchEventType.Deleted:
                    var deletedHandler = _ServiceProvider.GetService<WebPingerDeletedHandler>();
                    await deletedHandler.HandleAsync(pinger);
                    Console.WriteLine($"* Handled event: {type}, for: {pinger.Metadata.Name}");
                    break;

                default:
                    Console.WriteLine($"* Ignored event: {type}, for: {pinger.Metadata.Name}");
                    break;
            }
        }

        // TODO - move to config:
        public const string NamespaceName = "default";
    }
}
