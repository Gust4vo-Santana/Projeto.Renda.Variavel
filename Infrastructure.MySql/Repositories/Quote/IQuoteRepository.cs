using QuoteEntity = Domain.Entities.Quote;

namespace Infrastructure.MySql.Repositories.Quote
{
    public interface IQuoteRepository
    {
        Task<QuoteEntity?> GetLatestQuoteAsync(long assetId, CancellationToken cancellationToken);
        Task AddNewQuote(QuoteEntity newQuote, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}
