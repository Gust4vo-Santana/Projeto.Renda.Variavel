using Application.UseCases.Quote.AddNewQuote;

namespace Quotes.Consumer.AddNewQuoteWorkerService.Message
{
    public static class AddNewQuoteMessageMapper
    {
        public static AddNewQuoteInput MapToInput(this AddNewQuoteMessage message)
        {
            return new AddNewQuoteInput
            {
                AssetId = message.AssetId,
                Price = message.Price,
                Date = message.Date
            };
        }
    }
}
