using Application.UseCases.Operation.GetGlobalAveragePriceByAsset;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Operation
{
    public class GetGlobalAveragePriceByAssetUseCaseTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<IValidator<GetGlobalAveragePriceByAssetInput>> _validatorMock;
        private readonly Mock<ILogger<GetGlobalAveragePriceByAssetUseCase>> _loggerMock;
        private readonly GetGlobalAveragePriceByAssetUseCase _useCase;

        public GetGlobalAveragePriceByAssetUseCaseTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _validatorMock = new Mock<IValidator<GetGlobalAveragePriceByAssetInput>>();
            _loggerMock = new Mock<ILogger<GetGlobalAveragePriceByAssetUseCase>>();
            _useCase = new GetGlobalAveragePriceByAssetUseCase(
                _operationRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetGlobalAveragePriceByAssetInput { AssetId = 42 };
            var expectedWeightedSum = 123.99m;
            var expectedTotalQuantity = 100m;
            var expectedAverage = expectedWeightedSum / expectedTotalQuantity;
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _operationRepositoryMock
                .Setup(r => r.GetWeightedSumByAssetAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedWeightedSum);
            _operationRepositoryMock
                .Setup(r => r.GetTotalQuantityByAssetAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTotalQuantity);

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
            var input = new GetGlobalAveragePriceByAssetInput { AssetId = 0 };
            var validationFailures = new[] { new ValidationFailure("AssetId", "AssetId must be greater than zero") };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("AssetId must be greater than zero", output.GetErrorMessages()[0]);
        }

        [Fact]
        public async Task GivenRepositoryThrowsException_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithExceptionMessage()
        {
            // Arrange
            var input = new GetGlobalAveragePriceByAssetInput { AssetId = 42 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _operationRepositoryMock
                .Setup(r => r.GetWeightedSumByAssetAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}