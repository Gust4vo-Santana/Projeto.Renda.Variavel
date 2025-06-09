using Application.UseCases.Quote.AddNewQuote;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.MySql.Repositories.Quote;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using QuoteEntity = Domain.Entities.Quote;

namespace UnitTests.Application.UseCases.Quote
{
    public class AddNewQuoteUseCaseTests
    {
        private readonly Mock<IQuoteRepository> _quoteRepositoryMock;
        private readonly Mock<IValidator<AddNewQuoteInput>> _validatorMock;
        private readonly Mock<ILogger<AddNewQuoteUseCase>> _loggerMock;
        private readonly AddNewQuoteUseCase _useCase;

        public AddNewQuoteUseCaseTests()
        {
            _quoteRepositoryMock = new Mock<IQuoteRepository>();
            _validatorMock = new Mock<IValidator<AddNewQuoteInput>>();
            _loggerMock = new Mock<ILogger<AddNewQuoteUseCase>>();
            _useCase = new AddNewQuoteUseCase(
                _quoteRepositoryMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GivenValidInput_WhenExecuteAsyncIsCalled_ThenAddsNewQuote()
        {
            // Arrange
            var input = new AddNewQuoteInput { Id = 1, AssetId = 2, Price = 100.5m, Date = DateTime.UtcNow };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.ExistsAsync(input.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _quoteRepositoryMock
                .Setup(r => r.AddNewQuote(It.IsAny<QuoteEntity>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            _quoteRepositoryMock.Verify(r => r.AddNewQuote(It.IsAny<QuoteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsyncIsCalled_ThenLogsErrorAndDoesNotPersistNewQuote()
        {
            // Arrange
            var input = new AddNewQuoteInput { Id = 1, AssetId = 2, Price = -100.5m, Date = DateTime.UtcNow };
            var validationFailures = new[] { new ValidationFailure("Price", "Price must be positive") };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            await _useCase.ExecuteAsync(input, CancellationToken.None);
            
            // Assert
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while adding new quote")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((state, exception) => true)
                ),
                Times.Once
            );
            _quoteRepositoryMock.Verify(r => r.AddNewQuote(It.IsAny<QuoteEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GivenExistingQuoteId_WhenExecuteAsyncIsCalled_ThenLogsErrorAndDoesNotPersistNewQuote()
        {
            // Arrange
            var input = new AddNewQuoteInput { Id = 1, AssetId = 2, Price = 100.5m, Date = DateTime.UtcNow };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.ExistsAsync(input.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while adding new quote")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((state, exception) => true)
                ),
                Times.Once
            );
            _quoteRepositoryMock.Verify(r => r.AddNewQuote(It.IsAny<QuoteEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GivenRepositoryThrowsException_WhenExecuteAsyncIsCalled_ThenLogsError()
        {
            // Arrange
            var input = new AddNewQuoteInput { Id = 1, AssetId = 2, Price = 100.5m, Date = DateTime.UtcNow };
            _validatorMock
                .Setup(v => v.ValidateAsync(input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _quoteRepositoryMock
                .Setup(r => r.ExistsAsync(input.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _quoteRepositoryMock
                .Setup(r => r.AddNewQuote(It.IsAny<QuoteEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository error"));

            // Act
            await _useCase.ExecuteAsync(input, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while adding new quote")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}