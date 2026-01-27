using BuildingBlocks.Common.Entities;
using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuildingBlocks.Infrastructure.UnitOfWork;

/// <summary>
/// Unit of Work implementation using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
public class UnitOfWork<TContext> : IUnitOfWork where TContext : BaseDbContext
{
    private readonly TContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _repositories = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Gets a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity identifier type.</typeparam>
    /// <returns>The repository instance.</returns>
    public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : Entity<TId>
    {
        var type = typeof(IRepository<TEntity, TId>);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Repository<TEntity, TId>(_context);
            _repositories[type] = repositoryInstance;
        }

        return (IRepository<TEntity, TId>)_repositories[type];
    }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction to rollback.");
        }

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    /// <summary>
    /// Disposes the unit of work.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}