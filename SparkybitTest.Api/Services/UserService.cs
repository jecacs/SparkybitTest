using SparkybitTest.Domain.Models;
using SparkybitTest.Domain.Repositories;
using SparkybitTest.Domain.Services;
using SparkybitTest.RabbitMq.Contracts;

namespace SparkybitTest.Api.Services;

public sealed class UserService : IUserService
{
    private readonly IMessageBus _messageBus;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IMessageBus messageBus, IUserRepository userRepository, ILogger<UserService> logger)
    {
        _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger;
    }

    public Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(name))
        {
            return _messageBus.PublishAsync(new CreateUserContract(name), cancellationToken);
        }

        _logger.LogWarning("Name is null or empty");

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default)
        => _userRepository.GetUsersAsync(cancellationToken);
}