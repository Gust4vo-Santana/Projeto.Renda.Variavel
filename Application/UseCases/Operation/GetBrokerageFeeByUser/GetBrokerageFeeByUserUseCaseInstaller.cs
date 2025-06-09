using Application.UseCases.Operation.GetBrokerageFeeByUser.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UseCases.Operation.GetBrokerageFeeByUser
{
    public static class GetBrokerageFeeByUserUseCaseInstaller
    {
        public static IServiceCollection AddGetBrokerageFeeByUserUseCase(this IServiceCollection services)
        {
            services.AddScoped<IGetBrokerageFeeByUserUseCase, GetBrokerageFeeByUserUseCase>()
                    .AddSingleton<IValidator<GetBrokerageFeeByUserInput>, GetBrokerageFeeByUserInputValidator>();
            return services;
        }
    }
}
