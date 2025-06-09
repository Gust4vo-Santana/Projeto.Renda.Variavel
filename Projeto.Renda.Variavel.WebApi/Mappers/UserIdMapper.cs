using Projeto.Renda.Variavel.WebApi.Dtos;

namespace Projeto.Renda.Variavel.WebApi.Mappers
{
    public static class UserIdMapper
    {
        public static IEnumerable<UserIdDto> MapToDtoList(this IEnumerable<long> userIds)
        {
            return userIds.Select(userId => new UserIdDto
            {
                UserId = userId
            });
        }
    }
}
