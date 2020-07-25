using k8s;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserController.CustomResources;
using UserController.Handlers;

namespace UserController
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
                group: User.Definition.Group,
                version: User.Definition.Version,
                plural: User.Definition.Plural,
                namespaceParameter: "default",
                watch: true);            

            using (result.Watch<User, object>((type, item) => Handle(type, item)))
            {
                Console.WriteLine("* Watching for custom object events");
                _ResetEvent.WaitOne();
            }
        }

        public static void Handle(WatchEventType type, User user)
        {
            switch (type)
            {
                case WatchEventType.Added:
                    new UserAddedHandler(_Client).Handle(user);
                    Console.WriteLine($"* Handled event: {type}, for user: {user.Metadata.Name}");
                    break;

                case WatchEventType.Deleted:
                    new UserDeletedHandler(_Client).Handle(user);
                    Console.WriteLine($"* Handled event: {type}, for user: {user.Metadata.Name}");
                    break;

                default:
                    Console.WriteLine($"* Ignored event: {type}, for user: {user.Metadata.Name}");
                    break;
            }
            
        }
    }
}
