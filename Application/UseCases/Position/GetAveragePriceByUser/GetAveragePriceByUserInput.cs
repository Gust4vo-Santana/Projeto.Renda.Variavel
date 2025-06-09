namespace Application.UseCases.Position.GetAveragePriceByUser
{
    public record GetAveragePriceByUserInput
    {
        public long UserId { get; set; }
        public long AssetId { get; set; }
    }
}
