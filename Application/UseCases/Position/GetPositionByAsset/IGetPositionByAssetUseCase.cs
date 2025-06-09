using Application.Shared.Output;
using PositionEntity = Domain.Entities.Position;

namespace Application.UseCases.Position.GetPositionByAsset
{
    public interface IGetPositionByAssetUseCase
    {
        Task<Output<PositionEntity?>> ExecuteAsync(GetPositionByAssetInput input, CancellationToken cancellationToken);
    }
}
