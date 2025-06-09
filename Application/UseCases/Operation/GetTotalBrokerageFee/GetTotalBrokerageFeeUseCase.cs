using Application.Shared.Output;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Operation.GetTotalBrokerageFee
{
    public class GetTotalBrokerageFeeUseCase : IGetTotalBrokerageFeeUseCase
    {
        private readonly IOperationRepository _operationRepository;
        private readonly ILogger<GetTotalBrokerageFeeUseCase> _logger;

        public GetTotalBrokerageFeeUseCase(IOperationRepository operationRepository,
                                           ILogger<GetTotalBrokerageFeeUseCase> logger)
        {
            _operationRepository = operationRepository;
            _logger = logger;
        }

        public async Task<Output<decimal>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTotalBrokerageFeeUseCase started.");

            var output = new Output<decimal>();

            try
            {
                var result = await _operationRepository.GetTotalBrokerageFeeAsync(cancellationToken);

                _logger.LogInformation("GetTotalBrokerageFeeUseCase performed successfully.");

                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching total brokerage fee.");
                output.AddErrorMessage($"An error occurred while fetching total brokerage fee: {ex.Message}");

                return output;
            }
        }
    }
}
