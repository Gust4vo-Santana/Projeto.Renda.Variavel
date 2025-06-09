using Application.UseCases.Position.GetTopUsersWithHighestPositions;
using Infrastructure.MySql.Repositories.Position;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Position
{
    public class GetTopUsersWithHighestPositionsUseCaseTests
    {
        private readonly Mock<IPositionRepository> _positionRepositoryMock;
        private readonly Mock<ILogger<GetTopUsersWithHighestPositionsUseCase>> _loggerMock;
        private readonly GetTopUsersWithHighestPositionsUseCase _useCase;

        public GetTopUsersWithHighestPositionsUseCaseTests()
        {
            _positionRepositoryMock = new Mock<IPositionRepository>();
            _loggerMock = new Mock<ILogger<GetTopUsersWithHighestPositionsUseCase>>();
            _useCase = new GetTopUsersWithHighestPositionsUseCase(
                _positionRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenRepositoryReturnsUserIds_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var expectedUserIds = new List<long> { 10, 20, 30 };
            _positionRepositoryMock
                .Setup(r => r.GetTopUsersWithHighestPositionsAsync(It.IsAny<CancellationToken>()))
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
            _positionRepositoryMock
                .Setup(r => r.GetTopUsersWithHighestPositionsAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}