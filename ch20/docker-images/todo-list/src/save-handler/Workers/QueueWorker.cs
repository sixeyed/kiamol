using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using System;
using System.Threading;
using ToDoList.Messaging;
using ToDoList.Messaging.Messages.Events;
using ToDoList.Model;

namespace ToDoList.SaveHandler.Workers
{
    public class QueueWorker
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private const string QUEUE_GROUP = "save-handler";

        private readonly MessageQueue _messageQueue;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;

        public QueueWorker(MessageQueue messageQueue, IConfiguration config, IServiceProvider serviceProvider)
        {
            _messageQueue = messageQueue;
            _config = config;
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            Console.WriteLine($"Connecting to message queue url: {_config["MessageQueue:Url"]}");
            using (var connection = _messageQueue.CreateConnection())
            {
                var subscription = connection.SubscribeAsync(NewItemEvent.MessageSubject, QUEUE_GROUP);
                subscription.MessageHandler += SaveItem;
                subscription.Start();
                Console.WriteLine($"Listening on subject: {NewItemEvent.MessageSubject}, queue: {QUEUE_GROUP}");

                _ResetEvent.WaitOne();
                connection.Close();
            }
        }

        private void SaveItem(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine($"Received message, subject: {e.Message.Subject}");
            var eventMessage = MessageHelper.FromData<NewItemEvent>(e.Message.Data);
            Console.WriteLine($"Saving item, added: {eventMessage.Item.DateAdded}; event ID: {eventMessage.CorrelationId}");

            try
            {
                using (var context = _serviceProvider.GetService<ToDoContext>())
                {
                    context.ToDos.Add(eventMessage.Item);
                    context.SaveChanges();
                }
                if (bool.Parse(_config[$"Events:{ItemSavedEvent.MessageSubject}:Publish"]))
                {
                    _messageQueue.Publish(new ItemSavedEvent(eventMessage.Item));
                }
                Console.WriteLine($"Item saved; ID: {eventMessage.Item.ToDoId}; event ID: {eventMessage.CorrelationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save FAILED; event ID: {eventMessage.CorrelationId}; exception: {ex}");
            }
        }
    }
}
