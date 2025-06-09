using Application.UseCases.Quote.GetLatestQuote.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Quote.GetLatestQuote
{
    public static class GetLatestQuoteUseCaseInstaller
    {
        public static IServiceCollection AddGetLatestQuoteUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetLatestQuoteUseCase, GetLatestQuoteUseCase>()
                    .AddSingleton<IValidator<GetLatestQuoteInput>, GetLatestQuoteInputValidator>();
            
            return services;
        }
    }
}
