using Application.UseCases.Position.GetAveragePriceByUser;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Position
{
    public class GetAveragePriceByUserUseCaseTests
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly Mock<IValidator<GetAveragePriceByUserInput>> _validatorMock;
        private readonly Mock<ILogger<GetAveragePriceByUserUseCase>> _loggerMock;
        private readonly GetAveragePriceByUserUseCase _useCase;

        public GetAveragePriceByUserUseCaseTests()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _validatorMock = new Mock<IValidator<GetAveragePriceByUserInput>>();
            _loggerMock = new Mock<ILogger<GetAveragePriceByUserUseCase>>();
            _useCase = new GetAveragePriceByUserUseCase(
                _positionRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetAveragePriceByUserInput { UserId = 1, AssetId = 2 };
            var expectedAverage = 10.5m;
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetAveragePriceByUserAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAverage);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedAverage, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetAveragePriceByUserInput { UserId = 0, AssetId = 0 };
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
            var input = new GetAveragePriceByUserInput { UserId = 1, AssetId = 2 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetAveragePriceByUserAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}