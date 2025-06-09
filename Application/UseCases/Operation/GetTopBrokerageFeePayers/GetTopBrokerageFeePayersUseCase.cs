using Application.Shared.Output;
using Infrastructure.MySql.Repositories.Operation;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Operation.GetTopBrokerageFeePayers
{
    public class GetTopBrokerageFeePayersUseCase : IGetTopBrokerageFeePayersUseCase
    {
        private readonly IOperationRepository _operationRepository;
        private readonly ILogger<GetTopBrokerageFeePayersUseCase> _logger;

        public GetTopBrokerageFeePayersUseCase(IOperationRepository operationRepository,
                                               ILogger<GetTopBrokerageFeePayersUseCase> logger)
        {
            _operationRepository = operationRepository;
            _logger = logger;
        }

        public async Task<Output<IEnumerable<long>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTopBrokerageFeePayersUseCase started.");

            var output = new Output<IEnumerable<long>>();

            try
            {
                var result = await _operationRepository.GetTopBrokerageFeePayersAsync(cancellationToken);

                _logger.LogInformation("GetTopBrokerageFeePayersUseCase performed successfully.");

                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching top brokerage fee payers.");

                output.AddErrorMessage($"An error occurred while fetching top brokerage fee payers: {ex.Message}");
                return output;
            }
        }
    }
}
