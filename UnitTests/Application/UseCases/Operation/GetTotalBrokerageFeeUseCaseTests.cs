using Application.UseCases.Operation.GetTotalBrokerageFee;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Operation
{
    public class GetTotalBrokerageFeeUseCaseTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<ILogger<GetTotalBrokerageFeeUseCase>> _loggerMock;
        private readonly GetTotalBrokerageFeeUseCase _useCase;

        public GetTotalBrokerageFeeUseCaseTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _loggerMock = new Mock<ILogger<GetTotalBrokerageFeeUseCase>>();
            _useCase = new GetTotalBrokerageFeeUseCase(
                _operationRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenRepositoryReturnsTotal_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var expectedTotal = 1234.56m;
            _operationRepositoryMock
                .Setup(r => r.GetTotalBrokerageFeeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTotal);

            // Act
            var output = await _useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedTotal, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenRepositoryThrowsException_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithExceptionMessage()
        {
            // Arrange
            _operationRepositoryMock
                .Setup(r => r.GetTotalBrokerageFeeAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}