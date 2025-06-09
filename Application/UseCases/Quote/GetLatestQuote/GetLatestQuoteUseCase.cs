using Application.Shared.Output;
using FluentValidation;
using Infrastructure.MySql.Repositories.Quote;
using Microsoft.Extensions.Logging;
using QuoteEntity = Domain.Entities.Quote;

namespace Application.UseCases.Quote.GetLatestQuote
{
    public class GetLatestQuoteUseCase : IGetLatestQuoteUseCase
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IValidator<GetLatestQuoteInput> _validator;
        private readonly ILogger<GetLatestQuoteUseCase> _logger;

        public GetLatestQuoteUseCase(IQuoteRepository quoteRepository,
                                     IValidator<GetLatestQuoteInput> validator,
                                     ILogger<GetLatestQuoteUseCase> logger)
        {
            _quoteRepository = quoteRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<QuoteEntity?>> ExecuteAsync(GetLatestQuoteInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetLatestQuoteUseCase started for asset ID: {AssetId}", input.AssetId);

            var output = new Output<QuoteEntity?>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _quoteRepository.GetLatestQuoteAsync(input.AssetId, cancellationToken);

                if (result == null)
                {
                    output.AddErrorMessage("No quote found for the specified asset ID.");
                    return output;
                }

                output.AddResult(result);
                return output;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the latest quote for asset ID: {AssetId}", input.AssetId);

                output.AddErrorMessage($"An error occurred while fetching the latest quote: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetLatestQuoteInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
