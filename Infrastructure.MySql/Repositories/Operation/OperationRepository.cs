using Infrastructure.MySql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySql.Repositories.Operation
{
    public class OperationRepository : IOperationRepository
    {
        private readonly AppDbContext _context;

        public OperationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetBrokerageFeeByUserAsync(long userId, CancellationToken cancellationToken)
        {
            return await _context.Operations
                .Where(u => u.UserId == userId)
                .Select(u => u.BrokerageFee)
                .SumAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalBrokerageFeeAsync(CancellationToken cancellationToken)
        {
            return await _context.Operations
                .Select(u => u.BrokerageFee)
                .SumAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalQuantityByAssetAsync(long assetId, CancellationToken cancellationToken)
        {
            return await _context.Operations
                .Where(o => o.AssetId == assetId)
                .SumAsync(o => o.Quantity, cancellationToken);
        }

        public async Task<decimal> GetWeightedSumByAssetAsync(long assetId, CancellationToken cancellationToken)
        {
            return await _context.Operations
                .Where(o => o.AssetId == assetId)
                .Select(o => o.Price * o.Quantity)
                .SumAsync(cancellationToken);
        }

        public async Task<IEnumerable<long>> GetTopBrokerageFeePayersAsync(CancellationToken cancellationToken)
        {
            return await _context.Operations
                .GroupBy(o => o.UserId)
                .OrderByDescending(g => g.Sum(o => o.BrokerageFee))
                .Select(g => g.Key)
                .Take(10)
                .ToListAsync(cancellationToken);
        }
    }
}
