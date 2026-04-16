using MediatR;
using Customer.Application.IntegrationEvents;
using Customer.Domain.Events;
using BuildingBlocks.EventBus;
using Serilog;

namespace Customer.Application.Handlers;

/// <summary>
/// Handler for CustomerCreatedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class CustomerCreatedDomainEventHandler : INotificationHandler<CustomerCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public CustomerCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<CustomerCreatedDomainEventHandler>();
    }

    public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for customer creation: {CustomerId}", notification.CustomerId);

        var integrationEvent = new CustomerCreatedIntegrationEvent
        {
            CustomerId = notification.CustomerId,
            CompanyName = notification.CompanyName,
            Email = notification.Email,
            Phone = string.Empty,
            TaxId = string.Empty,
            CustomerType = string.Empty
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

/// <summary>
/// Handler for CustomerUpdatedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class CustomerUpdatedDomainEventHandler : INotificationHandler<CustomerUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public CustomerUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<CustomerUpdatedDomainEventHandler>();
    }

    public async Task Handle(CustomerUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for customer update: {CustomerId}", notification.CustomerId);

        var integrationEvent = new CustomerUpdatedIntegrationEvent
        {
            CustomerId = notification.CustomerId,
            CompanyName = notification.CompanyName,
            Email = string.Empty,
            Phone = string.Empty,
            CreditLimit = 0,
            UpdatedAt = DateTime.UtcNow
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

/// <summary>
/// Handler for CustomerDeletedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class CustomerDeletedDomainEventHandler : INotificationHandler<CustomerDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public CustomerDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<CustomerDeletedDomainEventHandler>();
    }

    public async Task Handle(CustomerDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for customer deletion: {CustomerId}", notification.CustomerId);

        var integrationEvent = new CustomerDeletedIntegrationEvent
        {
            CustomerId = notification.CustomerId,
            CompanyName = notification.CompanyName,
            DeletedAt = DateTime.UtcNow
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

/// <summary>
/// Handler for VendorCreatedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class VendorCreatedDomainEventHandler : INotificationHandler<VendorCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public VendorCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<VendorCreatedDomainEventHandler>();
    }

    public async Task Handle(VendorCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for vendor creation: {VendorId}", notification.VendorId);

        var integrationEvent = new VendorCreatedIntegrationEvent
        {
            VendorId = notification.VendorId,
            CompanyName = notification.CompanyName,
            Email = notification.Email,
            Phone = string.Empty,
            TaxId = string.Empty,
            VendorType = string.Empty
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

/// <summary>
/// Handler for VendorUpdatedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class VendorUpdatedDomainEventHandler : INotificationHandler<VendorUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public VendorUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<VendorUpdatedDomainEventHandler>();
    }

    public async Task Handle(VendorUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for vendor update: {VendorId}", notification.VendorId);

        var integrationEvent = new VendorUpdatedIntegrationEvent
        {
            VendorId = notification.VendorId,
            CompanyName = notification.CompanyName,
            Email = string.Empty,
            Phone = string.Empty,
            UpdatedAt = DateTime.UtcNow
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

/// <summary>
/// Handler for VendorDeletedDomainEvent that publishes the corresponding integration event.
/// </summary>
public class VendorDeletedDomainEventHandler : INotificationHandler<VendorDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public VendorDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<VendorDeletedDomainEventHandler>();
    }

    public async Task Handle(VendorDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Publishing integration event for vendor deletion: {VendorId}", notification.VendorId);

        var integrationEvent = new VendorDeletedIntegrationEvent
        {
            VendorId = notification.VendorId,
            CompanyName = notification.CompanyName,
            DeletedAt = DateTime.UtcNow
        };

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
