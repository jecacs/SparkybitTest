using SparkybitTest.Domain.Models;

namespace SparkybitTest.Api.Dto.Converters;

public static class UserConverters
{
    public static IReadOnlyCollection<UserDto> ToDto(this IReadOnlyCollection<UserModel> src)
        => src.Select(x => new UserDto(x.Id, x.Name, x.CreatedAt)).ToList();
}