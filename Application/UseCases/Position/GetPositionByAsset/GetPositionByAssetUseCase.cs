using Application.Shared.Output;
using FluentValidation;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using PositionEntity = Domain.Entities.Position;

namespace Application.UseCases.Position.GetPositionByAsset
{
    public class GetPositionByAssetUseCase : IGetPositionByAssetUseCase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IValidator<GetPositionByAssetInput> _validator;
        private readonly ILogger<GetPositionByAssetUseCase> _logger;

        public GetPositionByAssetUseCase(IPositionRepository positionRepository,
                                         IValidator<GetPositionByAssetInput> validator,
                                         ILogger<GetPositionByAssetUseCase> logger)
        {
            _positionRepository = positionRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<PositionEntity?>> ExecuteAsync(GetPositionByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetPositionByAssetUseCase started for user with ID: {UserId} and asset ID: {AssetId}", input.UserId, input.AssetId);

            var output = new Output<PositionEntity?>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _positionRepository.GetPositionByAssetAsync(input.UserId, input.AssetId, cancellationToken);
                
                _logger.LogInformation("GetPositionByAssetUseCase completed successfully for userId: {UserId} and assetId: {AssetId}", input.UserId, input.AssetId);
                
                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching position for userId: {UserId} and asset ID: {AssetId}", input.UserId, input.AssetId);
                
                output.AddErrorMessage($"An error occurred while fetching position: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetPositionByAssetInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
