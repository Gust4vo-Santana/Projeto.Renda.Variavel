namespace Projeto.Renda.Variavel.WebApi.Dtos
{
    public record QuoteDto
    {
        public Guid Id { get; set; }
        public long AssetId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
