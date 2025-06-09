using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Operation.GetTotalBrokerageFee
{
    public static class GetTotalBrokerageFeeUseCaseInstaller
    {
        public static IServiceCollection AddGetTotalBrokerageFeeUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetTotalBrokerageFeeUseCase, GetTotalBrokerageFeeUseCase>();
            return services;
        }
    }
}
