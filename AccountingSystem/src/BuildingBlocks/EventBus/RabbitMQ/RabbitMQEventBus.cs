using BuildingBlocks.Common.Extensions;
using BuildingBlocks.EventBus.Abstractions;
using BuildingBlocks.EventBus.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQ = RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.EventBus.RabbitMQ;

/// <summary>
/// RabbitMQ implementation of the event bus.
/// </summary>
public class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly RabbitMQConnection _connection;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly RabbitMQEventBusOptions _options;
    private readonly AsyncPolicy _combinedPolicy;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private RMQ.IModel? _consumerChannel;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMQEventBus"/> class.
    /// </summary>
    /// <param name="connection">The RabbitMQ connection.</param>
    /// <param name="subsManager">The subscriptions manager.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="options">The RabbitMQ options.</param>
    public RabbitMQEventBus(
        RabbitMQConnection connection,
        IEventBusSubscriptionsManager subsManager,
        IServiceProvider serviceProvider,
        ILogger<RabbitMQEventBus> logger,
        RabbitMQEventBusOptions options)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        _exchangeName = options.ExchangeName;
        _queueName = $"{options.QueueNamePrefix}_{Guid.NewGuid()}";

        _combinedPolicy = EventBusResiliencePolicies.GetCombinedPolicy(
            options.RetryCount,
            options.CircuitBreakerFailureThreshold,
            TimeSpan.FromMinutes(options.CircuitBreakerRecoveryTimeMinutes));

        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }

    /// <summary>
    /// Publishes an integration event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <param name="event">The integration event to publish.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await _combinedPolicy.ExecuteAsync(async () =>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Publishing event {EventId} of type {EventType}", @event.EventId, eventName);

            using var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true);

            var message = JsonSerializer.Serialize(@event, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // Persistent
            properties.MessageId = @event.EventId.ToString();
            properties.Timestamp = new AmqpTimestamp(@event.OccurredOn.ToUnixTimestamp());

            channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);
        });
    }

    /// <summary>
    /// Subscribes to an integration event with a handler.
    /// </summary>
    /// <typeparam name="T">The type of the integration event.</typeparam>
    /// <typeparam name="TH">The type of the event handler.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SubscribeAsync<T, TH>(CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();
        _logger.LogInformation("Subscribing to event {EventName} with handler {HandlerType}", eventName, typeof(TH).Name);

        _subsManager.AddSubscription<T, TH>();

        await DoInternalSubscriptionAsync(eventName, cancellationToken);
    }

    private async Task DoInternalSubscriptionAsync(string eventName, CancellationToken cancellationToken)
    {
        if (!_connection.IsConnected)
        {
            _connection.TryConnect();
        }

        _consumerChannel = _connection.CreateModel();

        _consumerChannel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
        _consumerChannel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: eventName);

        var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
        consumer.Received += async (model, ea) =>
        {
            var routingKey = ea.RoutingKey;
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            await ProcessEventAsync(routingKey, message, cancellationToken);

            // Acknowledge the message
            _consumerChannel.BasicAck(ea.DeliveryTag, false);
        };

        _consumerChannel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
    }

    private async Task ProcessEventAsync(string eventName, string message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing event {EventName}", eventName);

        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);

            foreach (var subscription in subscriptions)
            {
                var handler = _serviceProvider.GetService(subscription);
                if (handler == null)
                {
                    _logger.LogWarning("Handler {HandlerType} not found in service provider", subscription.Name);
                    continue;
                }

                var eventType = _subsManager.GetEventTypeByName(eventName);
                if (eventType == null)
                {
                    _logger.LogWarning("Event type for {EventName} not found", eventName);
                    continue;
                }

                var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                if (integrationEvent == null)
                {
                    _logger.LogWarning("Failed to deserialize event {EventName}", eventName);
                    continue;
                }

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                var handleMethod = concreteType.GetMethod("HandleAsync");
                if (handleMethod != null)
                {
                    await (Task)handleMethod.Invoke(handler, new[] { integrationEvent, cancellationToken })!;
                }
            }
        }
    }

    private void SubsManager_OnEventRemoved(object? sender, string eventName)
    {
        if (!_connection.IsConnected)
        {
            _connection.TryConnect();
        }

        using var channel = _connection.CreateModel();
        channel.QueueUnbind(queue: _queueName, exchange: _exchangeName, routingKey: eventName);

        if (_subsManager.IsEmpty)
        {
            _consumerChannel?.Close();
        }
    }

    /// <summary>
    /// Disposes the event bus.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        _subsManager.Clear();
        _consumerChannel?.Dispose();
    }
}