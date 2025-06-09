
using Infrastructure.MySql.Context;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.EntityFrameworkCore;
using PositionEntity = Domain.Entities.Position;

namespace Infrastructure.MySql.Repositories.User
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;

        public PositionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalInvestedByAssetAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            return await _context.Positions
                .Where(p => p.UserId == userId && p.AssetId == assetId)
                .Select(p => p.Quantity * p.AveragePrice)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PositionEntity>> GetGlobalPositionAsync(long userId, CancellationToken cancellationToken)
        {
            return await _context.Positions
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<decimal> GetAveragePriceByUserAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            return await _context.Positions
                .Where(p => p.UserId == userId && p.AssetId == assetId)
                .Select(p => p.AveragePrice)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PositionEntity?> GetPositionByAssetAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetId == assetId, cancellationToken);
        }

        public async Task<IEnumerable<long>> GetTopUsersWithHighestPositionsAsync(CancellationToken cancellationToken)
        {
            return await _context.Positions
                .GroupBy(p => p.UserId)
                .OrderByDescending(g => g.Sum(p => p.Quantity * p.AveragePrice))
                .Select(g => g.Key)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
    }
}
