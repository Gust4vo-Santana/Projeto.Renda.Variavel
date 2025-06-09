namespace Application.UseCases.Quote
{
    public record GetLatestQuoteInput
    {
        public long AssetId { get; init; }
    }
}
