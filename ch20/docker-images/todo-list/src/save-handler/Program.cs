using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Messaging;
using ToDoList.SaveHandler.Workers;

namespace ToDoList.SaveHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("config/logging.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config/config.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets/secrets.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<QueueWorker>()
                .AddSingleton<MessageQueue>()
                .AddToDoContext(config, ServiceLifetime.Transient)
                .BuildServiceProvider();

            var worker = serviceProvider.GetService<QueueWorker>();
            worker.Start();
        }
    }
}
