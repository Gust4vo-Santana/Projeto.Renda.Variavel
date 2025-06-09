using Application.Shared.Output;
using FluentValidation;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Position.GetTotalInvestedByAsset
{
    public class GetTotalInvestedByAssetUseCase : IGetTotalInvestedByAssetUseCase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IValidator<GetTotalInvestedByAssetInput> _validator;
        private readonly ILogger<GetTotalInvestedByAssetUseCase> _logger;

        public GetTotalInvestedByAssetUseCase(IPositionRepository positionRepository,
                                              IValidator<GetTotalInvestedByAssetInput> validator,     
                                              ILogger<GetTotalInvestedByAssetUseCase> logger)
        {
            _positionRepository = positionRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<decimal>> ExecuteAsync(GetTotalInvestedByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTotalInvestedByAssetUseCase started for userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);

            var output = new Output<decimal>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _positionRepository.GetTotalInvestedByAssetAsync(input.UserId, input.AssetId, cancellationToken);

                _logger.LogInformation("GetTotalInvestedByAssetUseCase performed successfully for userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);

                output.AddResult(result);
                return output;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching total invested for userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);
                
                output.AddErrorMessage($"An error occurred while fetching total invested by asset: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetTotalInvestedByAssetInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
