using BuildingBlocks.Common.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Specifications;

/// <summary>
/// Evaluates specifications and applies them to Entity Framework queries.
/// </summary>
public static class SpecificationEvaluator
{
    /// <summary>
    /// Gets the query with the specification applied.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="inputQuery">The input query.</param>
    /// <param name="specification">The specification to apply.</param>
    /// <returns>The query with the specification applied.</returns>
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification)
        where T : class
    {
        var query = inputQuery;

        // Apply criteria
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        // Apply includes
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply string includes
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        // Apply ordering
        if (specification.OrderByExpressions.Any())
        {
            var firstOrderBy = specification.OrderByExpressions.First();
            var orderedQuery = firstOrderBy.IsDescending
                ? query.OrderByDescending(firstOrderBy.OrderBy)
                : query.OrderBy(firstOrderBy.OrderBy);

            query = specification.OrderByExpressions.Skip(1)
                .Aggregate(orderedQuery, (current, orderBy) =>
                    orderBy.IsDescending
                        ? current.ThenByDescending(orderBy.OrderBy)
                        : current.ThenBy(orderBy.OrderBy));
        }

        // Apply paging
        if (specification.Skip.HasValue)
        {
            query = query.Skip(specification.Skip.Value);
        }

        if (specification.Take.HasValue)
        {
            query = query.Take(specification.Take.Value);
        }

        // Apply tracking
        if (!specification.IsTrackingEnabled)
        {
            query = query.AsNoTracking();
        }

        // Apply split query
        if (specification.IsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        // Apply ignore query filters
        if (specification.IsIgnoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        // Apply ignore auto includes
        if (specification.IsIgnoreAutoIncludes)
        {
            query = query.IgnoreAutoIncludes();
        }

        return query;
    }
}