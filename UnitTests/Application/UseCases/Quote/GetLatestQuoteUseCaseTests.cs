using Application.UseCases.Quote;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Quote;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using QuoteEntity = Domain.Entities.Quote;

namespace UnitTests.Application.UseCases.Quote
{
    public class GetLatestQuoteUseCaseTests
    {
        private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
        private readonly Mock<IValidator<GetLatestQuoteInput>> _validatorMock;
        private readonly Mock<ILogger<GetLatestQuoteUseCase>> _loggerMock;
        private readonly GetLatestQuoteUseCase _useCase;

        public GetLatestQuoteUseCaseTests()
        {
            _quoteRepositoryMock = new Mock<IQuoteRepository>();
            _validatorMock = new Mock<IValidator<GetLatestQuoteInput>>();
            _loggerMock = new Mock<ILogger<GetLatestQuoteUseCase>>();
            _useCase = new GetLatestQuoteUseCase(
                _quoteRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenQuoteExists_ThenReturnsValidOutputWithResult()
        {
            // Arrange
            var input = new GetLatestQuoteInput { AssetId = 1 };
            var expectedQuote = new QuoteEntity()
            {
                AssetId = input.AssetId,
                Price = 83.70m,
                Date = new DateTime()
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.GetLatestQuoteAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedQuote);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.True(output.IsValid);
            Assert.Equal(expectedQuote, output.GetResult());
            Assert.Empty(output.GetErrorMessages());
        }

        [Fact]
        public async Task GivenValidInput_WhenQuoteDoesNotExist_ThenReturnsInvalidOutputWithNoQuoteFoundMessage()
        {
            // Arrange
            var input = new GetLatestQuoteInput { AssetId = 1 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.GetLatestQuoteAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QuoteEntity?)null);

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("No quote found for the specified asset ID.", output.GetErrorMessages()[0]);
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenReturnsInvalidOutputWithErrorMessages()
        {
            // Arrange
            var input = new GetLatestQuoteInput { AssetId = 0 };
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
            var input = new GetLatestQuoteInput { AssetId = 1 };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.GetLatestQuoteAsync(input.AssetId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            var output = await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            Assert.False(output.IsValid);
            Assert.Contains("Repository error", output.GetErrorMessages()[0]);
        }
    }
}