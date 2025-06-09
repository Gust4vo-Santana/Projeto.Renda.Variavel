using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Shared.Output;
using Application.UseCases.Operation.GetTopBrokerageFeePayers;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Operation
{
    public class GetTopBrokerageFeePayersUseCaseTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<ILogger<GetTopBrokerageFeePayersUseCase>> _loggerMock;
        private readonly GetTopBrokerageFeePayersUseCase _useCase;

        public GetTopBrokerageFeePayersUseCaseTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _loggerMock = new Mock<ILogger<GetTopBrokerageFeePayersUseCase>>();
            _useCase = new GetTopBrokerageFeePayersUseCase(
                _operationRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenRepositoryReturnsUserIds_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var expectedUserIds = new List<long> { 1, 2, 3 };
            _operationRepositoryMock
                .Setup(r => r.GetTopBrokerageFeePayersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUserIds);

            // Act
            var output = await _useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedUserIds, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenRepositoryThrowsException_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithExceptionMessage()
        {
            // Arrange
            _operationRepositoryMock
                .Setup(r => r.GetTopBrokerageFeePayersAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}