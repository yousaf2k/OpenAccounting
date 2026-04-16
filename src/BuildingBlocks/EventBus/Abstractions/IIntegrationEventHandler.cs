namespace BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Interface for handling integration events.
/// </summary>
/// <typeparam name="TIntegrationEvent">The type of the integration event.</typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Handles the integration event asynchronously.
    /// </summary>
    /// <param name="event">The integration event to handle.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}