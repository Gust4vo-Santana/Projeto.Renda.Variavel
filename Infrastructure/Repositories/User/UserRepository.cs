
using Infrastructure.MySql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MySql.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<string, decimal>> GetTotalInvestedByAssetAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            var result = await _context.Positions
                .Where(p => p.UserId == userId && p.AssetId == assetId)
                .GroupBy(p => p.AssetId)
                .ToDictionaryAsync(g => g.Key.ToString(), 
                                   g => g.Sum(p => p.Quantity * p.AveragePrice));

            return result;
        }
    }
}
