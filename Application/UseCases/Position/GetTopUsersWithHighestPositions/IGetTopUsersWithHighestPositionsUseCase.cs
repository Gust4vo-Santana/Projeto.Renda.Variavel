using Application.Shared.Output;

namespace Application.UseCases.Position.GetTopUsersWithHighestPositions
{
    public interface IGetTopUsersWithHighestPositionsUseCase
    {
        Task<Output<IEnumerable<long>>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
