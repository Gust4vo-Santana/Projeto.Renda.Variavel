using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Position.GetTopUsersWithHighestPositions
{
    public static class GetTopUsersWithHighestPositionsUseCaseInstaller
    {
        public static IServiceCollection AddGetTopUsersWithHighestPositionsUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetTopUsersWithHighestPositionsUseCase, GetTopUsersWithHighestPositionsUseCase>();
            return services;
        }
    }
}
