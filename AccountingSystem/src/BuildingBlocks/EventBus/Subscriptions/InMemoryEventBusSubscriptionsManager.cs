using BuildingBlocks.EventBus.Abstractions;

namespace BuildingBlocks.EventBus.Subscriptions;

/// <summary>
/// In-memory implementation of the event bus subscriptions manager.
/// </summary>
public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    /// <summary>
    /// Occurs when an event is removed.
    /// </summary>
    public event EventHandler<string>? OnEventRemoved;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryEventBusSubscriptionsManager"/> class.
    /// </summary>
    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    /// <summary>
    /// Gets a value indicating whether there are subscriptions for the specified event.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>true if there are subscriptions; otherwise, false.</returns>
    public bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    /// <summary>
    /// Gets a value indicating whether there are subscriptions for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>true if there are subscriptions; otherwise, false.</returns>
    public bool HasSubscriptionsForEvent(string eventName)
    {
        return _handlers.ContainsKey(eventName);
    }

    /// <summary>
    /// Gets the event types.
    /// </summary>
    public IEnumerable<Type> EventTypes => _eventTypes;

    /// <summary>
    /// Gets the handlers for the specified event.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>The collection of handler types.</returns>
    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    /// <summary>
    /// Gets the handlers for the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The collection of handler types.</returns>
    public IEnumerable<Type> GetHandlersForEvent(string eventName)
    {
        return _handlers.ContainsKey(eventName)
            ? _handlers[eventName].Select(s => s.HandlerType)
            : new List<Type>();
    }

    /// <summary>
    /// Adds a subscription for the specified event and handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    public void AddSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, new List<SubscriptionInfo>());
        }

        if (_handlers[eventName].Any(s => s.HandlerType == typeof(TH)))
        {
            throw new ArgumentException($"Handler Type {typeof(TH).Name} already registered for '{eventName}'", nameof(TH));
        }

        _handlers[eventName].Add(new SubscriptionInfo(false, typeof(TH)));

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }

    /// <summary>
    /// Removes a subscription for the specified event and handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    public void RemoveSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        var subscriptionToRemove = FindSubscriptionToRemove(eventName, typeof(TH));

        if (subscriptionToRemove != null)
        {
            _handlers[eventName].Remove(subscriptionToRemove);

            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }

                OnEventRemoved?.Invoke(this, eventName);
            }
        }
    }

    /// <summary>
    /// Clears all subscriptions.
    /// </summary>
    public void Clear()
    {
        _handlers.Clear();
        _eventTypes.Clear();
    }

    /// <summary>
    /// Gets the event key for the specified event type.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <returns>The event key.</returns>
    public string GetEventKey<T>() where T : IIntegrationEvent
    {
        return typeof(T).Name;
    }

    /// <summary>
    /// Gets the event type by name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <returns>The event type.</returns>
    public Type? GetEventTypeByName(string eventName)
    {
        return _eventTypes.SingleOrDefault(t => t.Name == eventName);
    }

    /// <summary>
    /// Gets a value indicating whether the subscriptions manager is empty.
    /// </summary>
    public bool IsEmpty => !_handlers.Keys.Any();

    private SubscriptionInfo? FindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }
}