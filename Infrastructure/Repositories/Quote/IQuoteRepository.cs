using QuoteEntity = Domain.Entities.Quote;

namespace Infrastructure.MySql.Repositories.Quote
{
    public interface IQuoteRepository
    {
        Task<QuoteEntity?> GetLatestQuoteAsync(long assetId, CancellationToken cancellationToken);
    }
}
