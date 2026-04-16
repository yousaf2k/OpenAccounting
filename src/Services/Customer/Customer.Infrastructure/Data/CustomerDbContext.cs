using Microsoft.EntityFrameworkCore;
using Customer.Domain.Entities;
using Customer.Infrastructure.Events;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Domain;

namespace Customer.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext for Customer service.
/// </summary>
public class CustomerDbContext : BaseDbContext
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IDomainEventDispatcher domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>Gets or sets the customers DbSet.</summary>
    public DbSet<Customer> Customers { get; set; } = null!;

    /// <summary>Gets or sets the vendors DbSet.</summary>
    public DbSet<Vendor> Vendors { get; set; } = null!;

    /// <summary>Gets or sets the contacts DbSet.</summary>
    public DbSet<Contact> Contacts { get; set; } = null!;

    /// <summary>Gets or sets the addresses DbSet.</summary>
    public DbSet<Address> Addresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
    }

    /// <summary>
    /// Overrides SaveChangesAsync to dispatch domain events before committing changes.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect all aggregates with domain events
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.GetDomainEvents().Any())
            .Select(e => e.Entity)
            .ToList();

        // Dispatch domain events if dispatcher is available
        if (_domainEventDispatcher != null && aggregates.Count > 0)
        {
            await _domainEventDispatcher.DispatchAsync(aggregates, cancellationToken);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
