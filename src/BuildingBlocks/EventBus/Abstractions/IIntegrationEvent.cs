namespace BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Marker interface for integration events.
/// Integration events are used for communication between different microservices.
/// </summary>
public interface IIntegrationEvent
{
    /// <summary>
    /// Gets the unique identifier of the integration event.
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// Gets the date and time when the integration event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}