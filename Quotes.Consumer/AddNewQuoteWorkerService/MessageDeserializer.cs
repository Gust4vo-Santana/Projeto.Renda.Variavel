using Confluent.Kafka;
using System.Text.Json;

namespace Quotes.Consumer.AddNewQuoteWorkerService
{
    public class MessageDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<T>(data)!;
        }
    }
}
