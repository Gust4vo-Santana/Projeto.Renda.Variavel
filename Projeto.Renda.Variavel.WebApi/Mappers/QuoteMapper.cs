using Domain.Entities;
using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class QuoteMapper
    {
        public static QuoteDto MapToDto(this Quote quote)
        {
            return new QuoteDto
            {
                Id = quote.Id,
                AssetId = quote.AssetId,
                Price = quote.Price,
                Date = quote.Date
            };
        }
    }
}
