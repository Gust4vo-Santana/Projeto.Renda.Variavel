using Application.Shared.Output;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Position.GetTopUsersWithHighestPositions
{
    public class GetTopUsersWithHighestPositionsUseCase : IGetTopUsersWithHighestPositionsUseCase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<GetTopUsersWithHighestPositionsUseCase> _logger;

        public GetTopUsersWithHighestPositionsUseCase(IPositionRepository positionRepository,
                                                      ILogger<GetTopUsersWithHighestPositionsUseCase> logger)
        {
            _positionRepository = positionRepository;
            _logger = logger;
        }
        public async Task<Output<IEnumerable<long>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTopUsersWithHighestPositionsUseCase started.");

            var output = new Output<IEnumerable<long>>();

            try
            {
                var result = await _positionRepository.GetTopUsersWithHighestPositionsAsync(cancellationToken);
                
                _logger.LogInformation("GetTopUsersWithHighestPositionsUseCase performed successfully.");

                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching top users with highest positions.");
                
                output.AddErrorMessage($"An error occurred while fetching top users with highest positions: {ex.Message}");
                return output;
            }
        }
    }
}
