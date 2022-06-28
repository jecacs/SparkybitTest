using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace SparkybitTest.Adapters.MongoDb;

public sealed class MongoDbConnectionFactory
{
    private readonly ILogger<MongoDbConnectionFactory> _logger;

    public MongoDbConnectionFactory(ILogger<MongoDbConnectionFactory> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IMongoDatabase GetDatabase(string connectionString)
    {
        var mongoUrl = new MongoUrl(connectionString);

        return GetClient(mongoUrl).GetDatabase(mongoUrl.DatabaseName, new MongoDatabaseSettings());
    }

    private MongoClient GetClient(MongoUrl connectionString)
    {
        var mongoClientSettings = MongoClientSettings.FromUrl(connectionString);

        mongoClientSettings.ClusterConfigurator = cb =>
        {
            cb.Subscribe<CommandFailedEvent>(e =>
            {
                _logger.LogWarning(e.Failure, $"{e.CommandName} - {e.Duration.TotalMilliseconds}ms");
            });
        };

        return new MongoClient(mongoClientSettings);
    }
}