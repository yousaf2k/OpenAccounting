using BuildingBlocks.EventBus.Abstractions;

namespace BuildingBlocks.EventBus.Events;

/// <summary>
/// Base class for integration events.
/// Provides default implementation of IIntegrationEvent with EventId and OccurredOn.
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Gets the unique identifier of the integration event.
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// Gets the date and time when the integration event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
    /// </summary>
    protected IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEvent"/> class with a specific event ID.
    /// </summary>
    /// <param name="eventId">The unique identifier of the integration event.</param>
    protected IntegrationEvent(Guid eventId)
    {
        EventId = eventId;
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegrationEvent"/> class with a specific event ID and occurrence time.
    /// </summary>
    /// <param name="eventId">The unique identifier of the integration event.</param>
    /// <param name="occurredOn">The date and time when the integration event occurred.</param>
    protected IntegrationEvent(Guid eventId, DateTime occurredOn)
    {
        EventId = eventId;
        OccurredOn = occurredOn;
    }
}