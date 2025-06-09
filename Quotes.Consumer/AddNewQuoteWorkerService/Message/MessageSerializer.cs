using Confluent.Kafka;
using System.Text.Json;

namespace Quotes.Consumer.AddNewQuoteWorkerService.Message
{
    public class MessageSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
