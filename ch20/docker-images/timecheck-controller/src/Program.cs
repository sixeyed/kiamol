using k8s;
using System;
using System.Threading;
using System.Threading.Tasks;
using TimecheckController.CustomResources;
using TimecheckController.Handlers;

namespace TimecheckController
{
    class Program
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static Kubernetes _Client;

        static async Task Main(string[] args)
        {
            KubernetesClientConfiguration config;
            if (KubernetesClientConfiguration.IsInCluster())
            {
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            else
            {
                config = new KubernetesClientConfiguration { Host = "http://localhost:8001" };
            }

            _Client = new Kubernetes(config);

            var result = await _Client.ListNamespacedCustomObjectWithHttpMessagesAsync(
                group: Timecheck.Definition.Group,
                version: Timecheck.Definition.Version,
                plural: Timecheck.Definition.Plural,
                namespaceParameter: "default",
                watch: true);            

            using (result.Watch<Timecheck, object>(async (type, item) => await Handle(type, item)))
            {
                Console.WriteLine("* Watching for custom object events");
                _ResetEvent.WaitOne();
            }
        }

        public static async Task Handle(WatchEventType type, Timecheck timecheck)
        {
            switch (type)
            {
                case WatchEventType.Added:
                    await new TimecheckAddedHandler(_Client).HandleAsync(timecheck);
                    Console.WriteLine($"* Handled event: {type}, for Timecheck: {timecheck.Metadata.Name}");
                    break;

                case WatchEventType.Deleted:
                    await new TimecheckDeletedHandler(_Client).HandleAsync(timecheck);
                    Console.WriteLine($"* Handled event: {type}, for Timecheck: {timecheck.Metadata.Name}");
                    break;

                default:
                    Console.WriteLine($"* Ignored event: {type}, for Timecheck: {timecheck.Metadata.Name}");
                    break;
            }            
        }

        // TODO - move to config:
        public const string NamespaceName = "default";
    }
}
