using Application.UseCases.Operation.GetBrokerageFeeByUser;
using Application.UseCases.Operation.GetGlobalAveragePriceByAsset;
using Application.UseCases.Operation.GetTopBrokerageFeePayers;
using Application.UseCases.Operation.GetTotalBrokerageFee;
using Application.UseCases.Position.GetAveragePriceByUser;
using Application.UseCases.Position.GetGlobalPosition;
using Application.UseCases.Position.GetPositionByAsset;
using Application.UseCases.Position.GetTopUsersWithHighestPositions;
using Application.UseCases.Position.GetTotalInvestedByAsset;
using Application.UseCases.Quote;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared.Installers
{
    public static class UseCasesInstaller
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddGetTotalInvestedByAssetUseCase()
                    .AddGetGlobalPositionUseCase()
                    .AddGetLatestQuoteUseCase()
                    .AddGetAveragePriceUseCase()
                    .AddGetPositionByAssetUseCase()
                    .AddGetBrokerageFeeByUserUseCase()
                    .AddGetTotalBrokerageFeeUseCase()
                    .AddGetGlobalAveragePriceByAssetUseCase()
                    .AddGetTopUsersWithHighestPositionsUseCase()
                    .AddGetTopBrokerageFeePayersUseCase();

            return services;
        }
    }
}
