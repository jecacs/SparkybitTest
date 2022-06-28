namespace SparkybitTest.Domain.Services;

public interface IMessageBus
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class;

    Task SubscribeAsync<TMessage>(Func<TMessage, Task> func, CancellationToken cancellationToken = default);
}