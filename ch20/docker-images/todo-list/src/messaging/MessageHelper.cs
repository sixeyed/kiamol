using Newtonsoft.Json;
using ToDoList.Messaging.Messages;
using System;
using System.Text;

namespace ToDoList.Messaging
{
    public class MessageHelper
    {
        public static byte[] ToData<TMessage>(TMessage message)
            where TMessage : Message
        {
            var json = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(json);
        }

        public static TMessage FromData<TMessage>(byte[] data)
            where TMessage : Message
        {
            var json = Encoding.UTF8.GetString(data);
            return (TMessage)JsonConvert.DeserializeObject<TMessage>(json);
        }
    }
}
