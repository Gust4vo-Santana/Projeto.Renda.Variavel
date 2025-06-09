using Infrastructure.MySql.Repositories.Operation;
using Infrastructure.MySql.Repositories.Position;
using Infrastructure.MySql.Repositories.Quote;
using Infrastructure.MySql.Repositories.User;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MySql.Installers
{
    public static class RepositoriesInstaller
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            return services;
        }
    }
}
