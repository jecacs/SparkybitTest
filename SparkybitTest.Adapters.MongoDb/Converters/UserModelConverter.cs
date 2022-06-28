using SparkybitTest.Adapters.MongoDb.Models;
using SparkybitTest.Domain.Models;

namespace SparkybitTest.Adapters.MongoDb.Converters;

public static class UserModelConverter
{
    public static IReadOnlyCollection<UserModel> ToDomain(this IEnumerable<UserMongoModel> models)
        => models.Select(ToDomain).ToList();

    private static UserModel ToDomain(UserMongoModel src)
        => new()
        {
            Id = src.Id,
            Name = src.Name,
            CreatedAt = src.CreatedAt
        };
}