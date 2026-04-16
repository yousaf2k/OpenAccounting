using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQ = RabbitMQ.Client;

namespace BuildingBlocks.EventBus.RabbitMQ;

/// <summary>
/// Manages persistent connection to RabbitMQ.
/// </summary>
public class RabbitMQConnection : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private bool _disposed;

    /// <summary>
    /// Gets a value indicating whether the connection is established.
    /// </summary>
    public bool IsConnected => _connection?.IsOpen == true;

    /// <summary>
    /// Occurs when the connection is established.
    /// </summary>
    public event EventHandler? ConnectionRestored;

    /// <summary>
    /// Occurs when the connection is lost.
    /// </summary>
    public event EventHandler? ConnectionLost;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMQConnection"/> class.
    /// </summary>
    /// <param name="options">The RabbitMQ configuration options.</param>
    public RabbitMQConnection(RabbitMQEventBusOptions options)
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            VirtualHost = options.VirtualHost,
            DispatchConsumersAsync = true
        };

        // SSL configuration - commented out for compatibility with RabbitMQ 6.8.1
        // if (options.UseSsl)
        // {
        //     _connectionFactory.Ssl = new SslOption(options.HostName, enabled: true);
        // }
    }

    /// <summary>
    /// Tries to connect to RabbitMQ.
    /// </summary>
    /// <returns>true if the connection was established; otherwise, false.</returns>
    public bool TryConnect()
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.ConnectionBlocked += OnConnectionBlocked;
            _connection.ConnectionUnblocked += OnConnectionUnblocked;

            ConnectionRestored?.Invoke(this, EventArgs.Empty);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a model (channel) for the connection.
    /// </summary>
    /// <returns>The created model.</returns>
    public RMQ.IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connection is available.");
        }

        return _connection!.CreateModel();
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        ConnectionLost?.Invoke(this, EventArgs.Empty);
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        ConnectionLost?.Invoke(this, EventArgs.Empty);
    }

    private void OnConnectionUnblocked(object? sender, EventArgs e)
    {
        ConnectionRestored?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Disposes the connection.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        try
        {
            _connection?.Dispose();
        }
        catch (Exception)
        {
            // Ignore exceptions during disposal
        }
    }
}