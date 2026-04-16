using BuildingBlocks.EventBus.Abstractions;
using BuildingBlocks.EventBus.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.EventBus;

/// <summary>
/// Extension methods for adding Kafka event bus to the service collection.
/// </summary>
public static class KafkaEventBusServiceCollectionExtensions
{
    /// <summary>
    /// Adds Kafka event bus to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddKafkaEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var kafkaOptions = new KafkaEventBusOptions();
        configuration.GetSection("EventBus:Kafka").Bind(kafkaOptions);
        services.AddSingleton(kafkaOptions);

        services.AddSingleton<KafkaProducer>();
        services.AddSingleton<KafkaEventBus>();
        services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<KafkaEventBus>());

        services.AddHostedService<KafkaConsumer>();

        return services;
    }
}