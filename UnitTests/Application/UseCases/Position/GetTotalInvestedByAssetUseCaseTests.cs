using Application.UseCases.Position.GetTotalInvestedByAsset;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Position
{
    public class GetTotalInvestedByAssetUseCaseTests
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly Mock<IValidator<GetTotalInvestedByAssetInput>> _validatorMock;
        private readonly Mock<ILogger<GetTotalInvestedByAssetUseCase>> _loggerMock;
        private readonly GetTotalInvestedByAssetUseCase _useCase;

        public GetTotalInvestedByAssetUseCaseTests()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _validatorMock = new Mock<IValidator<GetTotalInvestedByAssetInput>>();
            _loggerMock = new Mock<ILogger<GetTotalInvestedByAssetUseCase>>();
            _useCase = new GetTotalInvestedByAssetUseCase(
                _positionRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetTotalInvestedByAssetInput { UserId = 1, AssetId = 2 };
            var expectedTotal = 1000.50m;
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetTotalInvestedByAssetAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTotal);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedTotal, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetTotalInvestedByAssetInput { UserId = 0, AssetId = 0 };
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
            var input = new GetTotalInvestedByAssetInput { UserId = 1, AssetId = 2 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetTotalInvestedByAssetAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}