using Application.UseCases.Position.GetAveragePriceByUser.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Position.GetAveragePriceByUser
{
    public static class GetAveragePriceByUserUseCaseInstaller
    {
        public static IServiceCollection AddGetAveragePriceUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetAveragePriceByUserUseCase, GetAveragePriceByUserUseCase>()
                    .AddSingleton<IValidator<GetAveragePriceByUserInput>, GetAveragePriceByUserInputValidator>();
            return services;
        }
    }
}
