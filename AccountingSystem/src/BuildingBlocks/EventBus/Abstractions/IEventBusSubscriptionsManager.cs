namespace BuildingBlocks.EventBus.Abstractions;

/// <summary>
/// Interface for managing event bus subscriptions.
/// </summary>
public interface IEventBusSubscriptionsManager
{
    /// <summary>
    /// Gets a value indicating whether there are subscriptions for the specified event.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>true if there are subscriptions; otherwise, false.</returns>
    bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent;

    /// <summary>
    /// Gets a value indicating whether there are subscriptions for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>true if there are subscriptions; otherwise, false.</returns>
    bool HasSubscriptionsForEvent(string eventName);

    /// <summary>
    /// Gets the event types.
    /// </summary>
    IEnumerable<Type> EventTypes { get; }

    /// <summary>
    /// Gets the handlers for the specified event.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>The collection of handler types.</returns>
    IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent;

    /// <summary>
    /// Gets the handlers for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The collection of handler types.</returns>
    IEnumerable<Type> GetHandlersForEvent(string eventName);

    /// <summary>
    /// Adds a subscription for the specified event and handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    void AddSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Removes a subscription for the specified event and handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    void RemoveSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Clears all subscriptions.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets the event key for the specified event type.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>The event key.</returns>
    string GetEventKey<T>() where T : IIntegrationEvent;

    /// <summary>
    /// Occurs when an event is removed.
    /// </summary>
    event EventHandler<string>? OnEventRemoved;

    /// <summary>
    /// Gets the event type by name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The event type, or null if not found.</returns>
    Type? GetEventTypeByName(string eventName);

    /// <summary>
    /// Gets a value indicating whether the subscriptions manager is empty.
    /// </summary>
    bool IsEmpty { get; }
}