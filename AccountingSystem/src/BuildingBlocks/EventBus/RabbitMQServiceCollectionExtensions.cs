using BuildingBlocks.EventBus.Abstractions;
using BuildingBlocks.EventBus.RabbitMQ;
using BuildingBlocks.EventBus.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EventBus;

/// <summary>
/// Extension methods for registering RabbitMQ event bus services.
/// </summary>
public static class RabbitMQServiceCollectionExtensions
{
    /// <summary>
    /// Adds RabbitMQ event bus services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="options">The RabbitMQ configuration options.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services, RabbitMQEventBusOptions options)
    {
        services.AddSingleton(options);
        services.AddSingleton<RabbitMQConnection>();
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, RabbitMQEventBus>();

        return services;
    }

    /// <summary>
    /// Adds RabbitMQ event bus services to the service collection with default options.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddRabbitMQEventBus(this IServiceCollection services)
    {
        return services.AddRabbitMQEventBus(new RabbitMQEventBusOptions());
    }
}