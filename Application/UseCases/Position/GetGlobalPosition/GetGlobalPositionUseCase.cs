using Application.Shared.Output;
using FluentValidation;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using PositionEntity = Domain.Entities.Position;

namespace Application.UseCases.Position.GetGlobalPosition
{
    public class GetGlobalPositionUseCase : IGetGlobalPositionUseCase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IValidator<GetGlobalPositionInput> _validator;
        private readonly ILogger<GetGlobalPositionUseCase> _logger;

        public GetGlobalPositionUseCase(IPositionRepository positionRepository,
                                        IValidator<GetGlobalPositionInput> validator,
                                        ILogger<GetGlobalPositionUseCase> logger)
        {
            _positionRepository = positionRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Output<IEnumerable<PositionEntity>>> ExecuteAsync(GetGlobalPositionInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetGlobalPositionUseCase started for user with ID: {UserId}", input.UserId);

            var output = new Output<IEnumerable<PositionEntity>>();

            try
            {
                await ValidateInputAsync(input, cancellationToken);

                var result = await _positionRepository.GetGlobalPositionAsync(input.UserId, cancellationToken);

                _logger.LogInformation("GetGlobalPositionUseCase performed successfully for user with ID: {UserId}", input.UserId); 

                output.AddResult(result);
                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching global position for user with ID: {UserId}", input.UserId);

                output.AddErrorMessage($"An error occurred while fetching global position: {ex.Message}");

                return output;
            }
        }

        private async Task ValidateInputAsync(GetGlobalPositionInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
