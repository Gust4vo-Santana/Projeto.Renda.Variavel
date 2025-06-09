using Application.Shared.Output;

namespace Application.UseCases.Operation.GetTopBrokerageFeePayers
{
    public interface IGetTopBrokerageFeePayersUseCase
    {
        Task<Output<IEnumerable<long>>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
