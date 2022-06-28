namespace SparkybitTest.Consumer.Services;

public class SubscribeHostedService : BackgroundService
{
    private readonly MessageBusSubscriberService _messageBusSubscriberService;
    private readonly ILogger<SubscribeHostedService> _logger;

    public SubscribeHostedService(MessageBusSubscriberService messageBusSubscriberService, ILogger<SubscribeHostedService> logger)
    {
        _messageBusSubscriberService = messageBusSubscriberService ?? throw new ArgumentNullException(nameof(messageBusSubscriberService));
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(10000, stoppingToken);

        _logger.LogWarning("SubscribeHostedService is started");

        await _messageBusSubscriberService.SubscribeAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("SubscribeHostedService is stopped");

        return Task.CompletedTask;
    }
}