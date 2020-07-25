using k8s;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;
using WebPingOperator.WebPingerArchiveController.Handlers;

namespace WebPingOperator.WebPingerArchiveController
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
                .AddTransient<WebPingerArchiveAddedHandler>()
                .AddTransient<WebPingerArchiveDeletedHandler>()
                .BuildServiceProvider();

            // TODO - this hangs and then fails if there are no objects
            var result = await kubernetes.ListNamespacedCustomObjectWithHttpMessagesAsync(
                            group: WebPingerArchive.Definition.Group,
                            version: WebPingerArchive.Definition.Version,
                            plural: WebPingerArchive.Definition.Plural,
                            namespaceParameter: "default",
                            watch: true);

            using (result.Watch<WebPingerArchive, object>(async (type, item) => await Handle(type, item)))
            {
                Console.WriteLine("* Watching for WebPingerArchive events");

                var ctrlc = new ManualResetEventSlim(false);
                Console.CancelKeyPress += (sender, eventArgs) => ctrlc.Set();
                ctrlc.Wait();
            }
        }

        public static async Task Handle(WatchEventType type, WebPingerArchive archive)
        {
            switch (type)
            {
                case WatchEventType.Added:
                    var addedhandler = _ServiceProvider.GetService<WebPingerArchiveAddedHandler>();
                    await addedhandler.HandleAsync(archive);
                    Console.WriteLine($"* Handled event: {type}, for: {archive.Metadata.Name}");
                    break;

                case WatchEventType.Deleted:
                    var deletedHandler = _ServiceProvider.GetService<WebPingerArchiveDeletedHandler>();
                    await deletedHandler.HandleAsync(archive);
                    Console.WriteLine($"* Handled event: {type}, for: {archive.Metadata.Name}");
                    break;

                default:
                    Console.WriteLine($"* Ignored event: {type}, for: {archive.Metadata.Name}");
                    break;
            }
        }

        // TODO - move to config:
        public const string NamespaceName = "default";
    }
}
