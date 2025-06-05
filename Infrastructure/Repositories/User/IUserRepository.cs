namespace Infrastructure.MySql.Repositories.User
{
    public interface IUserRepository
    {
        Task<IDictionary<string, decimal>> GetTotalInvestedByAssetAsync(long userId, long assetId, CancellationToken cancellationToken);
    }
}
