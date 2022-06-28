using SparkybitTest.Domain.Repositories;
using SparkybitTest.Domain.Services;
using SparkybitTest.RabbitMq.Contracts;

namespace SparkybitTest.Consumer.Services;

public class MessageBusSubscriberService
{
    private readonly IMessageBus _messageBus;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<MessageBusSubscriberService> _logger;

    public MessageBusSubscriberService(IMessageBus messageBus, IUserRepository userRepository, ILogger<MessageBusSubscriberService> logger)
    {
        _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger;
    }

    public Task SubscribeAsync(CancellationToken cancellationToken)
    {
        return _messageBus.SubscribeAsync<CreateUserContract>(message =>
        {
            // TODO Debug logs
            _logger.LogInformation("Got name from RabbitMQ: {Name}", message.Name);

            return _userRepository.AddUserAsync(message.Name, cancellationToken);
        }, cancellationToken);
    }
}