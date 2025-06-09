using Application.Shared.Output;

namespace Application.UseCases.Position.GetTotalInvestedByAsset
{
    public interface IGetTotalInvestedByAssetUseCase
    {
        Task<Output<decimal>> ExecuteAsync(GetTotalInvestedByAssetInput input, CancellationToken cancellationToken);
    }
}
