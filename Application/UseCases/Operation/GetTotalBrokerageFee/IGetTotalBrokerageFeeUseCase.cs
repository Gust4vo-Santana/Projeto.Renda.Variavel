using Application.Shared.Output;

namespace Application.UseCases.Operation.GetTotalBrokerageFee
{
    public interface IGetTotalBrokerageFeeUseCase
    {
        Task<Output<decimal>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
