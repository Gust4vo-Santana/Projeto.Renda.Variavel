namespace Application.UseCases.Quote.GetLatestQuote
{
    public record GetLatestQuoteInput
    {
        public long AssetId { get; init; }
    }
}
