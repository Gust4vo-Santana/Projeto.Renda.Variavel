namespace Application.UseCases.Quote.AddNewQuote
{
    public interface IAddNewQuoteUseCase
    {
        Task ExecuteAsync(AddNewQuoteInput input, CancellationToken cancellationToken);
    }
}
