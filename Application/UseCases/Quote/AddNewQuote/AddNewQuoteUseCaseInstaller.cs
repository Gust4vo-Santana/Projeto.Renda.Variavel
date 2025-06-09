using Application.UseCases.Quote.AddNewQuote.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Quote.AddNewQuote
{
    public static class AddNewQuoteUseCaseInstaller
    {
        public static IServiceCollection AddAddNewQuoteUseCase(this IServiceCollection services)
        {
            services.AddScoped<IAddNewQuoteUseCase, AddNewQuoteUseCase>()
                    .AddSingleton<IValidator<AddNewQuoteInput>, AddNewQuoteInputValidator>();
            return services;
        }
    }
}
