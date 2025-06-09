namespace Application.UseCases.Quote.AddNewQuote
{
    public record AddNewQuoteInput
    {
        public Guid Id { get; init; }
        public long AssetId { get; init; }
        public decimal Price { get; init; }
        public DateTime Date { get; init; }
    }
}
