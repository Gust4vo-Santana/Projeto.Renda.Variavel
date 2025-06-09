namespace Application.UseCases.Position.GetPositionByAsset
{
    public record GetPositionByAssetInput
    {
        public long UserId { get; init; }
        public long AssetId { get; init; }
    }
}
