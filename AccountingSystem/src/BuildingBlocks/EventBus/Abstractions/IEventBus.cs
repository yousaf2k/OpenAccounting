namespace BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Interface for event bus that handles publishing and subscribing to integration events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an integration event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <param name="event">The integration event to publish.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent;

    /// <summary>
    /// Subscribes to an integration event with a handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SubscribeAsync<T, TH>(CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}