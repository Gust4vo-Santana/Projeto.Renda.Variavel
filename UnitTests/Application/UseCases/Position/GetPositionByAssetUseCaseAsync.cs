using Application.UseCases.Position.GetPositionByAsset;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using PositionEntity = Domain.Entities.Position;

namespace UnitTests.Application.UseCases.Position
{
    public class GetPositionByAssetUseCaseAsyncTests
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly Mock<IValidator<GetPositionByAssetInput>> _validatorMock;
        private readonly Mock<ILogger<GetPositionByAssetUseCase>> _loggerMock;
        private readonly GetPositionByAssetUseCase _useCase;

        public GetPositionByAssetUseCaseAsyncTests()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _validatorMock = new Mock<IValidator<GetPositionByAssetInput>>();
            _loggerMock = new Mock<ILogger<GetPositionByAssetUseCase>>();
            _useCase = new GetPositionByAssetUseCase(
                _positionRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetPositionByAssetInput { UserId = 1, AssetId = 2 };
            var expectedPosition = new PositionEntity();
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetPositionByAssetAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPosition);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedPosition, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenValidInput_WhenNoPositionFound_ThenReturnsValidOutputWithNullResult()
        {
            // Arrange
            var input = new GetPositionByAssetInput { UserId = 1, AssetId = 2 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetPositionByAssetAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PositionEntity?)null);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Null(output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetPositionByAssetInput { UserId = 0, AssetId = 0 };
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
            var input = new GetPositionByAssetInput { UserId = 1, AssetId = 2 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _positionRepositoryMock
                .Setup(r => r.GetPositionByAssetAsync(input.UserId, input.AssetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}