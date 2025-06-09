using Domain.Entities;
using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class PositionMapper
    {
        public static IEnumerable<PositionDto> MapToDtoList(this IEnumerable<Position> positions)
        {
            return positions.Select(p => new PositionDto
            {
                Id = p.Id,
                UserId = p.UserId,
                AssetId = p.AssetId,
                Quantity = p.Quantity,
                AveragePrice = p.AveragePrice,
                PAndL = p.PAndL
            });
        }

        public static PositionDto MapToDto(this Position position)
        {
            return new PositionDto
            {
                Id = position.Id,
                UserId = position.UserId,
                AssetId = position.AssetId,
                Quantity = position.Quantity,
                AveragePrice = position.AveragePrice,
                PAndL = position.PAndL
            };
        }
    }
}
