
using Infrastructure.MySql.Context;

namespace Infrastructure.MySql.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<IDictionary<string, decimal>> GetTotalInvestedByAssetAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            var result = _context.Positions
                .Where(p => p.UserId == userId && p.AssetId == assetId)
                .GroupBy(p => p.AssetId)
                .ToDictionary(g => g.Key.ToString(), g => g.Sum(p => p.Quantity * p.AveragePrice));

            return Task.FromResult<IDictionary<string, decimal>>(result);
        }
    }
}
