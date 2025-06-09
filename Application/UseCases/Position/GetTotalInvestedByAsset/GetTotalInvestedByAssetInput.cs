namespace Application.UseCases.Position.GetTotalInvestedByAsset
{
    public record GetTotalInvestedByAssetInput
    {
        public long UserId { get; init; }
        public long AssetId { get; init; }
    }
}
