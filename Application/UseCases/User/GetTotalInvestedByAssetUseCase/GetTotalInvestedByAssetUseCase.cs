using Application.Shared.Output;
using Infrastructure.MySql.Repositories.User;

namespace Application.UseCases.User.GetTotalInvestedByAssetUseCase
{
    public class GetTotalInvestedByAssetUseCase : IGetTotalInvestedByAssetUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetTotalInvestedByAssetUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Output<IDictionary<string, decimal>>> ExecuteAsync(long userId, long assetId, CancellationToken cancellationToken)
        {
            var output = new Output<IDictionary<string, decimal>>();
            var result = await _userRepository.GetTotalInvestedByAssetAsync(userId, assetId, cancellationToken);

            output.AddResult(result);

            return output;
        }
    }
}
