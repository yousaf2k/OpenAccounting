using BuildingBlocks.Common.Entities;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Repositories;

/// <summary>
/// Generic repository interface for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TId">The entity identifier type.</typeparam>
public interface IRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>All entities.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The matching entities.</returns>
    Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paged result of entities.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The paged result.</returns>
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity with the specified identifier exists.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>true if the entity exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The count of matching entities.</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The added entity.</returns>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>true if the entity was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
}