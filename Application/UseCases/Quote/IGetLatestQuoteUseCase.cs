using Application.Shared.Output;
using QuoteEntity = Domain.Entities.Quote;

namespace Application.UseCases.Quote
{
    public interface IGetLatestQuoteUseCase
    {
        Task<Output<QuoteEntity?>> ExecuteAsync(GetLatestQuoteInput input, CancellationToken cancellationToken);
    }
}
