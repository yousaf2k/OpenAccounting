using MediatR;
using Serilog;
using BuildingBlocks.Domain;

namespace Customer.Infrastructure.Events;

/// <summary>
/// Dispatcher for publishing domain events as MediatR notifications.
/// This allows domain events to trigger corresponding handlers, including integration event publication.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches all domain events from the given aggregates.
    /// </summary>
    Task DispatchAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of IDomainEventDispatcher using MediatR.
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _mediator;
    private readonly ILogger _logger;

    public DomainEventDispatcher(IPublisher mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = Log.ForContext<DomainEventDispatcher>();
    }

    public async Task DispatchAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken = default)
    {
        foreach (var aggregate in aggregates)
        {
            var events = aggregate.GetDomainEvents().ToList();

            if (events.Count == 0)
                continue;

            _logger.Information("Dispatching {EventCount} domain events for aggregate {AggregateId}",
                events.Count, aggregate.Id);

            foreach (var @event in events)
            {
                _logger.Debug("Publishing domain event: {EventType}", @event.GetType().Name);
                await _mediator.Publish(@event, cancellationToken);
            }

            aggregate.ClearDomainEvents();
        }
    }
}
