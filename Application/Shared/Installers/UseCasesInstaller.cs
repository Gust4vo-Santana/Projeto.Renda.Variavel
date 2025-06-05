using Application.UseCases.User.GetTotalInvestedByAssetUseCase;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared.Installers
{
    public static class UseCasesInstaller
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddGetTotalInvestedByAssetUseCase();
            return services;
        }
    }
}
