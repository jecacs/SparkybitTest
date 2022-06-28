using EasyNetQ;
using Microsoft.Extensions.Logging;
using SparkybitTest.Domain.Services;
using SparkybitTest.RabbitMq.Configs;

namespace SparkybitTest.RabbitMq.Services;

public sealed class RabbitMqMessageBus : IMessageBus, IDisposable
{
    private readonly ILogger<RabbitMqMessageBus> _logger;
    private readonly IBus _bus;
    public RabbitMqMessageBus(
        ILogger<RabbitMqMessageBus> logger,
        RabbitMqConfiguration configuration)
    {
        _logger = logger;
        _bus = RabbitHutch.CreateBus(configuration.EasyNetQConnectionString ?? throw new ArgumentNullException(nameof(configuration)));
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
    {
        try
        {
            await _bus.PubSub.PublishAsync(message, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public async Task SubscribeAsync<TMessage>(Func<TMessage, Task> func, CancellationToken cancellationToken = default)
    {
        try
        {
            // Id of container or kubeNode
            var subscriptionId = Guid.NewGuid().ToString();

            await _bus.PubSub.SubscribeAsync(subscriptionId, func, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    public void Dispose()
    {
        try
        {
            _bus.Dispose();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}