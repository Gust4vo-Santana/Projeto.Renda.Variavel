using Application.UseCases.Quote.AddNewQuote.Mapper;
using FluentValidation;
using Infrastructure.MySql.Repositories.Quote;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Quote.AddNewQuote
{
    public class AddNewQuoteUseCase : IAddNewQuoteUseCase
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IValidator<AddNewQuoteInput> _validator;
        private readonly ILogger<AddNewQuoteUseCase> _logger;

        public AddNewQuoteUseCase(IQuoteRepository quoteRepository,
                                  IValidator<AddNewQuoteInput> validator,
                                  ILogger<AddNewQuoteUseCase> logger)
        {
            _quoteRepository = quoteRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task ExecuteAsync(AddNewQuoteInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("AddNewQuoteUseCase started for assetId: {AssetId}", input.AssetId);

            try
            {
                await ValidateInputAsync(input, cancellationToken);
                
                await _quoteRepository.AddNewQuote(input.MapToDomainEntity(), cancellationToken);

                _logger.LogInformation("AddNewQuote completed successfully for assetId: {AssetId}", input.AssetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new quote for asset ID: {AssetId}", input.AssetId);
            }
        }

        private async Task ValidateInputAsync(AddNewQuoteInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
