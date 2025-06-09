using Application.UseCases.Position.GetGlobalPosition;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PositionEntity = Domain.Entities.Position;

namespace UnitTests.Application.UseCases.Position
{
    public class GetGlobalPositionUseCaseTests
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly Mock<IValidator<GetGlobalPositionInput>> _validatorMock;
        private readonly Mock<ILogger<GetGlobalPositionUseCase>> _loggerMock;
        private readonly GetGlobalPositionUseCase _useCase;

        public GetGlobalPositionUseCaseTests()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _validatorMock = new Mock<IValidator<GetGlobalPositionInput>>();
            _loggerMock = new Mock<ILogger<GetGlobalPositionUseCase>>();
            _useCase = new GetGlobalPositionUseCase(
                _positionRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetGlobalPositionInput { UserId = 1 };
            var expectedPositions = new List<PositionEntity>
            {
                new PositionEntity()
                {
                    AssetId = 1,
                    UserId = input.UserId,
                    Quantity = 100,
                    AveragePrice = 50.0m
                },
                new PositionEntity()
                {
                    AssetId = 2,
                    UserId = input.UserId,
                    Quantity = 256,
                    AveragePrice = 102.27m
                },
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetGlobalPositionAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPositions);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedPositions, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetGlobalPositionInput { UserId = 0 };
            var validationFailures = new[] { new ValidationFailure("UserId", "UserId must be greater than zero") };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("UserId must be greater than zero", output.GetErrorMessages()[0]);
        }

        [Fact]
        public async Task GivenRepositoryThrowsException_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithExceptionMessage()
        {
            // Arrange
            var input = new GetGlobalPositionInput{ UserId = 1 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetGlobalPositionAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}