using Application.Shared.Output;
using Application.UseCases.Quote;
using FluentValidation;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Operation.GetGlobalAveragePriceByAsset
{
    public class GetGlobalAveragePriceByAssetUseCase : IGetGlobalAveragePriceByAssetUseCase
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IValidator<GetGlobalAveragePriceByAssetInput> _validator;
        private readonly ILogger<GetGlobalAveragePriceByAssetUseCase> _logger;

        public GetGlobalAveragePriceByAssetUseCase(IOperationRepository operationRepository,
                                                   IValidator<GetGlobalAveragePriceByAssetInput> validator,
                                                   ILogger<GetGlobalAveragePriceByAssetUseCase> logger)
        {
            _operationRepository = operationRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<decimal>> ExecuteAsync(GetGlobalAveragePriceByAssetInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetGlobalAveragePriceByAssetUseCase started for assetId: {AssetId}", input.AssetId);

            var output = new Output<decimal>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var weightedSum = await _operationRepository.GetWeightedSumByAssetAsync(input.AssetId, cancellationToken);
                var totalQuantity = await _operationRepository.GetTotalQuantityByAssetAsync(input.AssetId, cancellationToken);

                _logger.LogInformation("GetGlobalAveragePriceByAssetUseCase performed successfully for assetId: {AssetId}", input.AssetId);

                output.AddResult(weightedSum / totalQuantity);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching global average price for assetId: {AssetId}", input.AssetId);

                output.AddErrorMessage($"An error occurred while fetching average price: {ex.Message}");
                return output;
            }
        }

        private async Task ValidateInputAsync(GetGlobalAveragePriceByAssetInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
