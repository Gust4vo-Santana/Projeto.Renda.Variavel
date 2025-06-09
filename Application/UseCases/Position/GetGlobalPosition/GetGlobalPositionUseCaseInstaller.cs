using Application.UseCases.Position.GetGlobalPosition.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Position.GetGlobalPosition
{
    public static class GetGlobalPositionUseCaseInstaller
    {
        public static IServiceCollection AddGetGlobalPositionUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetGlobalPositionUseCase, GetGlobalPositionUseCase>()
                    .AddSingleton<IValidator<GetGlobalPositionInput>, GetGlobalPositionInputValidator>();
            return services;
        }
    }
}
