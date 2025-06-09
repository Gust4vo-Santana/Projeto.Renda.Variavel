using Application.UseCases.Operation.GetBrokerageFeeByUser;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Operation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.Operation
{
    public class GetBrokerageFeeByUserUseCaseTests
    {
        private readonly Mock<IOperationRepository> _operationRepositoryMock;
        private readonly Mock<IValidator<GetBrokerageFeeByUserInput>> _validatorMock;
        private readonly Mock<ILogger<GetBrokerageFeeByUserUseCase>> _loggerMock;
        private readonly GetBrokerageFeeByUserUseCase _useCase;

        public GetBrokerageFeeByUserUseCaseTests()
        {
            _operationRepositoryMock = new Mock<IOperationRepository>();
            _validatorMock = new Mock<IValidator<GetBrokerageFeeByUserInput>>();
            _loggerMock = new Mock<ILogger<GetBrokerageFeeByUserUseCase>>();
            _useCase = new GetBrokerageFeeByUserUseCase(
                _operationRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetBrokerageFeeByUserInput { UserId = 123 };
            var expectedFee = 150.75m;
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _operationRepositoryMock
                .Setup(r => r.GetBrokerageFeeByUserAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFee);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedFee, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetBrokerageFeeByUserInput { UserId = 0 };
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
            var input = new GetBrokerageFeeByUserInput { UserId = 123 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _operationRepositoryMock
                .Setup(r => r.GetBrokerageFeeByUserAsync(input.UserId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}