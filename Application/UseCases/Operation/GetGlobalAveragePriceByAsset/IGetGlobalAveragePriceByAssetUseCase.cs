using Application.Shared.Output;

namespace Application.UseCases.Operation.GetGlobalAveragePriceByAsset
{
    public interface IGetGlobalAveragePriceByAssetUseCase
    {
        Task<Output<decimal>> ExecuteAsync(GetGlobalAveragePriceByAssetInput input, CancellationToken cancellationToken);
    }
}
