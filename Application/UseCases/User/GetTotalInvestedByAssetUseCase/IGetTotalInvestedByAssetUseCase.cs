using Application.Shared.Output;

namespace Application.UseCases.User.GetTotalInvestedByAssetUseCase
{
    public interface IGetTotalInvestedByAssetUseCase
    {
        Task<Output<IDictionary<string, decimal>>> ExecuteAsync(long userId, long assetId, CancellationToken cancellationToken);
    }
}
