using BuildingBlocks.Common.Entities;
using BuildingBlocks.Common.Interfaces;
using BuildingBlocks.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace BuildingBlocks.Infrastructure.Persistence;

/// <summary>
/// Base DbContext for all microservices.
/// Provides common functionality like audit tracking, soft deletes, and domain event dispatching.
/// </summary>
public abstract class BaseDbContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    /// <param name="mediator">The mediator for dispatching domain events.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="dateTime">The date time service.</param>
    protected BaseDbContext(
        DbContextOptions options,
        IMediator mediator,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
        : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events
        await DispatchDomainEventsAsync(cancellationToken);

        // Apply audit information
        ApplyAuditInformation();

        // Apply soft delete
        ApplySoftDelete();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker
            .Entries()
            .Where(x => x.Entity is IHasDomainEvents entityWithEvents && entityWithEvents.DomainEvents.Any())
            .Select(x => (IHasDomainEvents)x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        // Clear domain events to prevent duplicate dispatching
        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }

    private void ApplyAuditInformation()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTime.Now;
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    entry.Entity.UpdatedBy = _currentUserService.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    entry.Entity.UpdatedBy = _currentUserService.UserId;
                    break;
            }
        }
    }

    private void ApplySoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = _dateTime.Now;
                entry.Entity.DeletedBy = _currentUserService.UserId;
            }
        }
    }

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply common configurations
        modelBuilder.ApplyAuditableEntityConfiguration();
        modelBuilder.ApplySoftDeleteQueryFilter();
        modelBuilder.ApplyDecimalPrecisionConfiguration();
    }
}