using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class AveragePriceMapper
    {
        public static AveragePriceDto MapToAveragePriceDto(this decimal averagePrice)
        {
            return new AveragePriceDto
            {
                AveragePrice = averagePrice
            };
        }
    }
}
