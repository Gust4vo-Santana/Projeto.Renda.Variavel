using Infrastructure.MySql.Context;
using Microsoft.EntityFrameworkCore;
using QuoteEntity = Domain.Entities.Quote;

namespace Infrastructure.MySql.Repositories.Quote
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _context;

        public QuoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuoteEntity?> GetLatestQuoteAsync(long assetId, CancellationToken cancellationToken)
        {
            return await _context.Quotes
                .Where(q => q.AssetId == assetId)
                .OrderByDescending(q => q.Date)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
