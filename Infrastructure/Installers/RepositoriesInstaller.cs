using Infrastructure.MySql.Repositories.User;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MySql.Installers
{
    public static class RepositoriesInstaller
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
