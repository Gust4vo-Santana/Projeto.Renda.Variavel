using PositionEntity = Domain.Entities.Position;

namespace Infrastructure.MySql.Repositories.Position
{
    public interface IPositionRepository
    {
        Task<decimal> GetTotalInvestedByAssetAsync(long userId, long assetId, CancellationToken cancellationToken);
        Task<IEnumerable<PositionEntity>> GetGlobalPositionAsync(long userId, CancellationToken cancellationToken);
        Task<decimal> GetAveragePriceByUserAsync(long userId, long assetId, CancellationToken cancellationToken);
        Task<PositionEntity?> GetPositionByAssetAsync(long userId, long assetId, CancellationToken cancellationToken);
        Task<IEnumerable<long>> GetTopUsersWithHighestPositionsAsync(CancellationToken cancellationToken);
    }
}
