using Application.UseCases.Operation.GetGlobalAveragePriceByAsset.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Operation.GetGlobalAveragePriceByAsset
{
    public static class GetGlobalAveragePriceByAssetUseCaseInstaller
    {
        public static IServiceCollection AddGetGlobalAveragePriceByAssetUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetGlobalAveragePriceByAssetUseCase, GetGlobalAveragePriceByAssetUseCase>()
                    .AddSingleton<IValidator<GetGlobalAveragePriceByAssetInput>, GetGlobalAveragePriceInputValidator>();
            return services;
        }
    }
}
