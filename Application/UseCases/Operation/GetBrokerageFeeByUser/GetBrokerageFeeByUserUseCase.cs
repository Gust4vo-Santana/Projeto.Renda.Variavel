using Application.Shared.Output;
using Application.UseCases.Quote;
using FluentValidation;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Operation.GetBrokerageFeeByUser
{
    public class GetBrokerageFeeByUserUseCase : IGetBrokerageFeeByUserUseCase
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IValidator<GetBrokerageFeeByUserInput> _validator;
        private readonly ILogger<GetBrokerageFeeByUserUseCase> _logger;

        public GetBrokerageFeeByUserUseCase(IOperationRepository operationRepository,
                                            IValidator<GetBrokerageFeeByUserInput> validator,
                                            ILogger<GetBrokerageFeeByUserUseCase> logger)
        {
            _operationRepository = operationRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<decimal>> ExecuteAsync(GetBrokerageFeeByUserInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetBrokerageFeeByUserUseCase started for userId: {UserId}", input.UserId);

            var output = new Output<decimal>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _operationRepository.GetBrokerageFeeByUserAsync(input.UserId, cancellationToken);

                _logger.LogInformation("GetBrokerageFeeByUserUseCase performed successfully for userId: {UserId}", input.UserId);

                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching brokerage fee for user with ID: {UserId}", input.UserId);

                output.AddErrorMessage($"An error occurred while fetching brokerage fee: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetBrokerageFeeByUserInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
