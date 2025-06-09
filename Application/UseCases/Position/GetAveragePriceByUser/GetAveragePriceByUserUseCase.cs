using Application.Shared.Output;
using FluentValidation;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Position.GetAveragePriceByUser
{
    public class GetAveragePriceByUserUseCase : IGetAveragePriceByUserUseCase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IValidator<GetAveragePriceByUserInput> _validator;
        private readonly ILogger<GetAveragePriceByUserUseCase> _logger;

        public GetAveragePriceByUserUseCase(IPositionRepository positionRepository,
                                      IValidator<GetAveragePriceByUserInput> validator,
                                      ILogger<GetAveragePriceByUserUseCase> logger)
        {
            _positionRepository = positionRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<decimal>> ExecuteAsync(GetAveragePriceByUserInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAveragePriceUseCase started for user ID: {UserId} and asset ID: {AssetId}", input.UserId, input.AssetId);

            var output = new Output<decimal>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _positionRepository.GetAveragePriceByUserAsync(input.UserId, input.AssetId, cancellationToken);

                _logger.LogInformation("GetAveragePriceUseCase performed successfully for user ID: {UserId} and asset ID: {AssetId}", input.UserId, input.AssetId);

                output.AddResult(result);
                return output;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching average price for user ID: {UserId} and asset ID: {AssetId}", input.UserId, input.AssetId);

                output.AddErrorMessage($"An error occurred while fetching average price: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetAveragePriceByUserInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
