using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Operation.GetTopBrokerageFeePayers
{
    public static class GetTopBrokerageFeePayersUseCaseInstaller
    {
        public static IServiceCollection AddGetTopBrokerageFeePayersUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetTopBrokerageFeePayersUseCase, GetTopBrokerageFeePayersUseCase>();
            return services;
        }
    }
}
