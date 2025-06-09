namespace Infrastructure.MySql.Repositories.Operation
{
    public interface IOperationRepository
    {
        Task<decimal> GetBrokerageFeeByUserAsync(long userId, CancellationToken cancellationToken);
        Task<decimal> GetTotalBrokerageFeeAsync(CancellationToken cancellationToken);
        Task<decimal> GetTotalQuantityByAssetAsync(long assetId, CancellationToken cancellationToken);
        Task<decimal> GetWeightedSumByAssetAsync(long assetId, CancellationToken cancellationToken);
        Task<IEnumerable<long>> GetTopBrokerageFeePayersAsync(CancellationToken cancellationToken);
    }
}
