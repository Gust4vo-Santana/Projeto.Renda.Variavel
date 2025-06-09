using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Quote.AddNewQuote
{
    public static class AddNewQuoteUseCaseInstaller
    {
        public static IServiceCollection AddAddNewQuoteUseCase(this IServiceCollection services)
        {
            services.AddScoped<IAddNewQuoteUseCase, AddNewQuoteUseCase>();
            return services;
        }
    }
}
