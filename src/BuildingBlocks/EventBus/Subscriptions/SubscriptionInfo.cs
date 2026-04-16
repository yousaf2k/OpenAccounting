namespace BuildingBlocks.EventBus.Subscriptions;

/// <summary>
/// Holds information about an event subscription.
/// </summary>
public class SubscriptionInfo
{
    /// <summary>
    /// Gets a value indicating whether the subscription is dynamic.
    /// </summary>
    public bool IsDynamic { get; }

    /// <summary>
    /// Gets the handler type.
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionInfo"/> class.
    /// </summary>
    /// <param name="isDynamic">A value indicating whether the subscription is dynamic.</param>
    /// <param name="handlerType">The handler type.</param>
    public SubscriptionInfo(bool isDynamic, Type handlerType)
    {
        IsDynamic = isDynamic;
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }
}