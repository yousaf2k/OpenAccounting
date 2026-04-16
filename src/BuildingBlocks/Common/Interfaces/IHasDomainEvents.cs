using MediatR;

namespace BuildingBlocks.Common.Interfaces;

/// <summary>
/// Interface for entities that have domain events.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Gets the collection of domain events that have occurred.
    /// </summary>
    IReadOnlyCollection<INotification> DomainEvents { get; }

    /// <summary>
    /// Clears all domain events from the collection.
    /// </summary>
    void ClearDomainEvents();
}