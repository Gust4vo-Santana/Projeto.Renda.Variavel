using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class TotalBrokerageFeeMapper
    {
        public static BrokerageFeeDto MapToBrokerageFeeDto(this decimal brokerageFee)
        {
            return new BrokerageFeeDto
            {
                BrokerageFee = brokerageFee
            };
        }
    }
}
