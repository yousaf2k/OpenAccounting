using BuildingBlocks.EventBus.Abstractions;
using BuildingBlocks.EventBus.Resilience;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Polly;
using System.Text.Json;

namespace BuildingBlocks.EventBus.Kafka;

/// <summary>
/// Kafka producer for publishing events.
/// </summary>
public class KafkaProducer : IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly AsyncPolicy _combinedPolicy;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaProducer"/> class.
    /// </summary>
    /// <param name="options">The Kafka options.</param>
    /// <param name="logger">The logger.</param>
    public KafkaProducer(KafkaEventBusOptions options, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MaxInFlight = 5,
            MessageTimeoutMs = 30000,
            RetryBackoffMs = 1000,
            ReconnectBackoffMs = 1000
        };

        if (options.UseSsl)
        {
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.SaslMechanism = SaslMechanism.Plain;
            config.SaslUsername = options.SaslUsername;
            config.SaslPassword = options.SaslPassword;
        }

        _producer = new ProducerBuilder<string, string>(config).Build();

        _combinedPolicy = EventBusResiliencePolicies.GetCombinedPolicy(
            options.RetryCount,
            options.CircuitBreakerFailureThreshold,
            options.CircuitBreakerRecoveryTime);
    }

    /// <summary>
    /// Publishes an event to Kafka.
    /// </summary>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _combinedPolicy.ExecuteAsync(async () =>
        {
            var message = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            var kafkaMessage = new Message<string, string>
            {
                Key = @event.EventId.ToString(),
                Value = message,
                Headers = new Headers
                {
                    new Header("event-type", System.Text.Encoding.UTF8.GetBytes(@event.GetType().Name))
                }
            };

            var deliveryResult = await _producer.ProduceAsync(@event.GetType().Name, kafkaMessage, cancellationToken);

            _logger.LogInformation("Event {EventType} published to Kafka topic {Topic} at offset {Offset}",
                @event.GetType().Name, deliveryResult.Topic, deliveryResult.Offset);
        });
    }

    /// <summary>
    /// Disposes the producer.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;

        _producer?.Dispose();
        _disposed = true;
    }
}