using Application.UseCases.Quote.AddNewQuote;
using Confluent.Kafka;

namespace Quotes.Consumer.AddNewQuoteWorkerService.Message
{
    public static class AddNewQuoteMessageMapper
    {
        public static AddNewQuoteInput MapToInput(this Message<Guid, AddNewQuoteMessage> message)
        {
            return new AddNewQuoteInput
            {
                Id = message.Key,
                AssetId = message.Value.AssetId,
                Price = message.Value.Price,
                Date = message.Value.Date
            };
        }
    }
}
