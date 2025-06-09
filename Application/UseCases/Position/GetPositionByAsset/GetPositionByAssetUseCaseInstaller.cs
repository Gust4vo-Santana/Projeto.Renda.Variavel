using Application.UseCases.Position.GetGlobalPosition.Validator;
using Application.UseCases.Position.GetPositionByAsset.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Position.GetPositionByAsset
{
    public static class GetPositionByAssetUseCaseInstaller
    {
        public static IServiceCollection AddGetPositionByAssetUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetPositionByAssetUseCase, GetPositionByAssetUseCase>()
                    .AddSingleton<IValidator<GetPositionByAssetInput>, GetPositionByAssetInputValidator>();
            return services;
        }
    }
}
