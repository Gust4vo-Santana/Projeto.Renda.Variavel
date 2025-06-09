using Application.Shared.Output;

namespace Application.UseCases.Position.GetAveragePriceByUser
{
    public interface IGetAveragePriceByUserUseCase
    {
        Task<Output<decimal>> ExecuteAsync(GetAveragePriceByUserInput input, CancellationToken cancellationToken);
    }
}
