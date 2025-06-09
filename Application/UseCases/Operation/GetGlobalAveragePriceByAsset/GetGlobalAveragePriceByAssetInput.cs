namespace Application.UseCases.Operation.GetGlobalAveragePriceByAsset
{
    public record GetGlobalAveragePriceByAssetInput
    {
        public long AssetId { get; init; }
    }
}
