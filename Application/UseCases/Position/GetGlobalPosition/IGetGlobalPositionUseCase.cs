using Application.Shared.Output;
using PositionEntity = Domain.Entities.Position;

namespace Application.UseCases.Position.GetGlobalPosition
{
    public interface IGetGlobalPositionUseCase
    {
        Task<Output<IEnumerable<PositionEntity>>> ExecuteAsync(GetGlobalPositionInput input, CancellationToken cancellationToken);
    }
}
