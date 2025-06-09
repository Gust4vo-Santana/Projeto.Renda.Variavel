using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class TotalInvestedMapper
    {
        public static TotalInvestedDto MapToTotalInvestedDto(this decimal totalInvested)
        {
            return new TotalInvestedDto
            {
                TotalInvested = totalInvested
            };
        }
    }
}
