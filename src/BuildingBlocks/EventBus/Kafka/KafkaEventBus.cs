using BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EventBus.Kafka;

/// <summary>
/// Kafka implementation of the event bus.
/// </summary>
public class KafkaEventBus : IEventBus
{
    private readonly KafkaProducer _producer;
    private readonly ILogger<KafkaEventBus> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaEventBus"/> class.
    /// </summary>
    /// <param name="producer">The Kafka producer.</param>
    /// <param name="logger">The logger.</param>
    public KafkaEventBus(KafkaProducer producer, ILogger<KafkaEventBus> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    /// <summary>
    /// Publishes an integration event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <param name="event">The integration event to publish.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        _logger.LogInformation("Publishing event {EventId} of type {EventType}", @event.EventId, typeof(T).Name);

        await _producer.PublishAsync(@event, cancellationToken);

        _logger.LogInformation("Event {EventType} published successfully", typeof(T).Name);
    }

    /// <summary>
    /// Subscribes to an integration event with a handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task SubscribeAsync<T, TH>(CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        // Kafka subscriptions are handled by the consumer
        _logger.LogInformation("Subscription registered for event {EventType}", typeof(T).Name);
        return Task.CompletedTask;
    }
}