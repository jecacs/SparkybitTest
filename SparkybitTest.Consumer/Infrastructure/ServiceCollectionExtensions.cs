using MongoDB.Driver;
using SparkybitTest.Adapters.MongoDb;
using SparkybitTest.Adapters.MongoDb.Repositories;
using SparkybitTest.Consumer.Services;
using SparkybitTest.Domain.Repositories;
using SparkybitTest.Domain.Services;
using SparkybitTest.RabbitMq.Configs;
using SparkybitTest.RabbitMq.Services;

namespace SparkybitTest.Consumer.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(_ =>
            new RabbitMqConfiguration
            {
                EasyNetQConnectionString = builder.Configuration["RabbitMqConfiguration:EasyNetQConnectionString"]
            });
    }
    
    public static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

        // mongoDb
        builder.Services.AddSingleton<MongoDbConnectionFactory>();
        builder.Services.AddSingleton<IMongoDatabase>(ctx =>
        {
            var mongoFactory = ctx.GetRequiredService<MongoDbConnectionFactory>();

            return mongoFactory.GetDatabase(builder.Configuration["MongoDb:ConnectionString"]);
        });
        builder.Services.AddTransient<IUserRepository, MongoDbUserRepository>();

        builder.Services.AddSingleton<MessageBusSubscriberService>();
    }

    public static void AddHostedServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<SubscribeHostedService>();
    }
}