using Application.UseCases.Position.GetTotalInvestedByAsset.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Position.GetTotalInvestedByAsset
{
    public static class GetTotalInvestedByAssetUseCaseInstaller
    {
        public static IServiceCollection AddGetTotalInvestedByAssetUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetTotalInvestedByAssetUseCase, GetTotalInvestedByAssetUseCase>()
                    .AddSingleton<IValidator<GetTotalInvestedByAssetInput>, GetTotalInvestedByAssetInputValidator>();
            return services;
        }
    }
}
