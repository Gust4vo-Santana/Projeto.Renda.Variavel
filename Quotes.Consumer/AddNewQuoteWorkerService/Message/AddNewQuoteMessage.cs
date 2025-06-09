namespace Quotes.Consumer.AddNewQuoteWorkerService.Message
{
    public record AddNewQuoteMessage
    {
        public long Id { get; init; }
        public long AssetId { get; init; }
        public decimal Price { get; init; }
        public DateTime Date { get; init; }
    }
}
