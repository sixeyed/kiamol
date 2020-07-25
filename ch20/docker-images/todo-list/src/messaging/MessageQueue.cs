using Microsoft.Extensions.Configuration;
using NATS.Client;
using ToDoList.Messaging.Messages;

namespace ToDoList.Messaging
{
    public class MessageQueue
    {
        public IConfiguration _config;

        public MessageQueue(IConfiguration config)
        {
            _config = config;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : Message
        {
            using (var connection = CreateConnection())
            {
                var data = MessageHelper.ToData(message);
                connection.Publish(message.Subject, data);
            }
        }

        public IConnection CreateConnection()
        {
            return new ConnectionFactory().CreateConnection(_config["MessageQueue:Url"]);
        }
    }
}
