using BuildingBlocks.EventBus.Abstractions;
using BuildingBlocks.EventBus.Resilience;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System.Text.Json;

namespace BuildingBlocks.EventBus.Kafka;

/// <summary>
/// Kafka consumer for processing events.
/// </summary>
public class KafkaConsumer : BackgroundService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly KafkaEventBusOptions _options;
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly AsyncPolicy _combinedPolicy;
    private IConsumer<string, string> _consumer;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="KafkaConsumer"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="options">The Kafka options.</param>
    /// <param name="logger">The logger.</param>
    public KafkaConsumer(IServiceProvider serviceProvider, KafkaEventBusOptions options, ILogger<KafkaConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _logger = logger;

        _combinedPolicy = EventBusResiliencePolicies.GetCombinedPolicy(
            options.RetryCount,
            options.CircuitBreakerFailureThreshold,
            options.CircuitBreakerRecoveryTime);
    }

    /// <summary>
    /// Executes the consumer.
    /// </summary>
    /// <param name="stoppingToken">The stopping token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _options.BootstrapServers,
            GroupId = _options.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            SessionTimeoutMs = 30000,
            HeartbeatIntervalMs = 3000,
            MaxPollIntervalMs = 300000
        };

        if (_options.UseSsl)
        {
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.SaslMechanism = SaslMechanism.Plain;
            config.SaslUsername = _options.SaslUsername;
            config.SaslPassword = _options.SaslPassword;
        }

        _consumer = new ConsumerBuilder<string, string>(config).Build();

        _consumer.Subscribe(_options.TopicName);

        _logger.LogInformation("Kafka consumer started for topic {Topic}", _options.TopicName);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult != null)
                    {
                        await ProcessMessageAsync(consumeResult, stoppingToken);
                        _consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consuming message from Kafka");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Kafka consumer execution");
        }
        finally
        {
            _consumer?.Close();
        }
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult, CancellationToken cancellationToken)
    {
        await _combinedPolicy.ExecuteAsync(async () =>
        {
            var eventTypeHeader = consumeResult.Message.Headers.FirstOrDefault(h => h.Key == "event-type");
            if (eventTypeHeader == null)
            {
                _logger.LogWarning("Received message without event-type header");
                return;
            }

            var eventTypeName = System.Text.Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
            var eventType = Type.GetType(eventTypeName);

            if (eventType == null)
            {
                _logger.LogWarning("Unknown event type: {EventType}", eventTypeName);
                return;
            }

            var @event = JsonSerializer.Deserialize(consumeResult.Message.Value, eventType) as IIntegrationEvent;
            if (@event == null)
            {
                _logger.LogWarning("Failed to deserialize event of type {EventType}", eventTypeName);
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await eventBus.PublishAsync(@event, cancellationToken);

            _logger.LogInformation("Processed event {EventType} from Kafka", eventTypeName);
        });
    }

    /// <summary>
    /// Disposes the consumer.
    /// </summary>
    public new void Dispose()
    {
        if (_disposed) return;

        _consumer?.Dispose();
        base.Dispose();
        _disposed = true;
    }
}