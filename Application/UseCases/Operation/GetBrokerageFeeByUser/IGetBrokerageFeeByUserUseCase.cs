using Application.Shared.Output;

namespace Application.UseCases.Operation.GetBrokerageFeeByUser
{
    public interface IGetBrokerageFeeByUserUseCase
    {
        Task<Output<decimal>> ExecuteAsync(GetBrokerageFeeByUserInput input, CancellationToken cancellationToken);
    }
}
