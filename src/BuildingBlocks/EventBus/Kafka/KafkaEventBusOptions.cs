namespace BuildingBlocks.EventBus.Kafka;

/// <summary>
/// Options for configuring the Kafka event bus.
/// </summary>
public class KafkaEventBusOptions
{
    /// <summary>
    /// Gets or sets the bootstrap servers.
    /// </summary>
    public string BootstrapServers { get; set; } = "localhost:9092";

    /// <summary>
    /// Gets or sets the group id for the consumer.
    /// </summary>
    public string GroupId { get; set; } = "accounting-system";

    /// <summary>
    /// Gets or sets the topic name for events.
    /// </summary>
    public string TopicName { get; set; } = "accounting-events";

    /// <summary>
    /// Gets or sets a value indicating whether to use SSL.
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Gets or sets the SASL username.
    /// </summary>
    public string? SaslUsername { get; set; }

    /// <summary>
    /// Gets or sets the SASL password.
    /// </summary>
    public string? SaslPassword { get; set; }

    /// <summary>
    /// Gets or sets the retry count for operations.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the circuit breaker failure threshold.
    /// </summary>
    public int CircuitBreakerFailureThreshold { get; set; } = 5;

    /// <summary>
    /// Gets or sets the circuit breaker recovery time.
    /// </summary>
    public TimeSpan CircuitBreakerRecoveryTime { get; set; } = TimeSpan.FromSeconds(30);
}