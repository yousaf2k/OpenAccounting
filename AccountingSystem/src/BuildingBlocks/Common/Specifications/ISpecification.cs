using System.Linq.Expressions;

namespace BuildingBlocks.Common.Specifications;

/// <summary>
/// Defines a specification pattern interface for querying entities.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Gets the criteria expression for filtering entities.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the list of include expressions for eager loading related entities.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Gets the list of string-based include expressions for eager loading related entities.
    /// </summary>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Gets the list of order by expressions for sorting.
    /// </summary>
    List<(Expression<Func<T, object>> OrderBy, bool IsDescending)> OrderByExpressions { get; }

    /// <summary>
    /// Gets the number of entities to skip.
    /// </summary>
    int? Skip { get; }

    /// <summary>
    /// Gets the number of entities to take.
    /// </summary>
    int? Take { get; }

    /// <summary>
    /// Gets a value indicating whether to track entities in the query.
    /// </summary>
    bool IsTrackingEnabled { get; }

    /// <summary>
    /// Gets a value indicating whether to split queries for better performance.
    /// </summary>
    bool IsSplitQuery { get; }

    /// <summary>
    /// Gets a value indicating whether to ignore query filters.
    /// </summary>
    bool IsIgnoreQueryFilters { get; }

    /// <summary>
    /// Gets a value indicating whether to ignore auto includes.
    /// </summary>
    bool IsIgnoreAutoIncludes { get; }

    /// <summary>
    /// Adds an include expression.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> AddInclude(Expression<Func<T, object>> includeExpression);

    /// <summary>
    /// Adds a string-based include expression.
    /// </summary>
    /// <param name="includeString">The include string.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> AddInclude(string includeString);

    /// <summary>
    /// Adds an order by expression.
    /// </summary>
    /// <param name="orderByExpression">The order by expression.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> AddOrderBy(Expression<Func<T, object>> orderByExpression);

    /// <summary>
    /// Adds an order by descending expression.
    /// </summary>
    /// <param name="orderByDescendingExpression">The order by descending expression.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression);

    /// <summary>
    /// Applies pagination.
    /// </summary>
    /// <param name="skip">The number of entities to skip.</param>
    /// <param name="take">The number of entities to take.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> ApplyPaging(int skip, int take);

    /// <summary>
    /// Enables or disables entity tracking.
    /// </summary>
    /// <param name="isTrackingEnabled">Whether to enable tracking.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> EnableTracking(bool isTrackingEnabled = true);

    /// <summary>
    /// Enables or disables split queries.
    /// </summary>
    /// <param name="isSplitQuery">Whether to enable split queries.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> EnableSplitQuery(bool isSplitQuery = true);

    /// <summary>
    /// Enables or disables ignoring query filters.
    /// </summary>
    /// <param name="isIgnoreQueryFilters">Whether to ignore query filters.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> IgnoreQueryFilters(bool isIgnoreQueryFilters = true);

    /// <summary>
    /// Enables or disables ignoring auto includes.
    /// </summary>
    /// <param name="isIgnoreAutoIncludes">Whether to ignore auto includes.</param>
    /// <returns>The specification instance.</returns>
    ISpecification<T> IgnoreAutoIncludes(bool isIgnoreAutoIncludes = true);
}