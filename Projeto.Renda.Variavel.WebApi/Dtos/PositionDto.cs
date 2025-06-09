namespace Projeto.Renda.Variavel.WebApi.Dtos
{
    public record PositionDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AssetId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal PAndL { get; set; }
    }
}
