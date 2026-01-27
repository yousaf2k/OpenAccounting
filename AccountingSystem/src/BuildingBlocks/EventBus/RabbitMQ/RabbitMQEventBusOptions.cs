namespace BuildingBlocks.EventBus.RabbitMQ;

/// <summary>
/// Configuration options for RabbitMQ event bus.
/// </summary>
public class RabbitMQEventBusOptions
{
    /// <summary>
    /// Gets or sets the RabbitMQ host name.
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the RabbitMQ port.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Gets or sets the RabbitMQ user name.
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the RabbitMQ password.
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the exchange name.
    /// </summary>
    public string ExchangeName { get; set; } = "accounting_exchange";

    /// <summary>
    /// Gets or sets the queue name prefix.
    /// </summary>
    public string QueueNamePrefix { get; set; } = "accounting_queue";

    /// <summary>
    /// Gets or sets the retry count for failed operations.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets the circuit breaker failure threshold.
    /// </summary>
    public int CircuitBreakerFailureThreshold { get; set; } = 5;

    /// <summary>
    /// Gets or sets the circuit breaker recovery time in minutes.
    /// </summary>
    public int CircuitBreakerRecoveryTimeMinutes { get; set; } = 1;

    /// <summary>
    /// Gets or sets the connection retry delay in seconds.
    /// </summary>
    public int ConnectionRetryDelaySeconds { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether to use SSL.
    /// </summary>
    public bool UseSsl { get; set; } = false;

    /// <summary>
    /// Gets or sets the virtual host.
    /// </summary>
    public string VirtualHost { get; set; } = "/";
}