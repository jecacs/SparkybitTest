using MongoDB.Driver;
using SparkybitTest.Adapters.MongoDb.Converters;
using SparkybitTest.Adapters.MongoDb.Models;
using SparkybitTest.Domain.Models;
using SparkybitTest.Domain.Repositories;

namespace SparkybitTest.Adapters.MongoDb.Repositories;

public class MongoDbUserRepository : IUserRepository
{
    private readonly IMongoCollection<UserMongoModel> _userCollection;

    public MongoDbUserRepository(IMongoDatabase db)
    {
        var mongoDatabase = db ?? throw new ArgumentNullException(nameof(db));

        _userCollection = mongoDatabase.GetCollection<UserMongoModel>(UserMongoModel.COLLECTION_NAME);
    }

    public Task AddUserAsync(string name, CancellationToken cancellationToken = default)
    {
        var model = new UserMongoModel
        {
            Name = name,
            CreatedAt = DateTime.UtcNow
        };

        return _userCollection.InsertOneAsync(model, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var asyncCursor = await _userCollection.FindAsync(_ => true, cancellationToken: cancellationToken);

        var result = await asyncCursor.ToListAsync(cancellationToken);

        return result.ToDomain();
    }
}