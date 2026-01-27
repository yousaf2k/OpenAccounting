using BuildingBlocks.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Infrastructure.Interceptors;

/// <summary>
/// Interceptor for automatically setting audit properties on entities.
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly string? _currentUserId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditInterceptor"/> class.
    /// </summary>
    /// <param name="currentUserId">The current user identifier.</param>
    public AuditInterceptor(string? currentUserId = null)
    {
        _currentUserId = currentUserId;
    }

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ApplyAuditProperties(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ApplyAuditProperties(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditProperties(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = _currentUserId;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = _currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = _currentUserId;
                    break;
            }
        }
    }
}