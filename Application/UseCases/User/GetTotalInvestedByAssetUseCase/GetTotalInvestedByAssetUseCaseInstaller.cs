using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.User.GetTotalInvestedByAssetUseCase
{
    public static class GetTotalInvestedByAssetUseCaseInstaller
    {
        public static IServiceCollection AddGetTotalInvestedByAssetUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetTotalInvestedByAssetUseCase, GetTotalInvestedByAssetUseCase>();
            return services;
        }
    }
}
