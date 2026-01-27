using System.Linq.Expressions;

namespace BuildingBlocks.Common.Specifications;

/// <summary>
/// Base implementation of the specification pattern.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
    /// </summary>
    protected BaseSpecification()
    {
        Includes = new List<Expression<Func<T, object>>>();
        IncludeStrings = new List<string>();
        OrderByExpressions = new List<(Expression<Func<T, object>> OrderBy, bool IsDescending)>();
        IsTrackingEnabled = true;
        IsSplitQuery = false;
        IsIgnoreQueryFilters = false;
        IsIgnoreAutoIncludes = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with criteria.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    protected BaseSpecification(Expression<Func<T, bool>> criteria)
        : this()
    {
        Criteria = criteria;
    }

    /// <inheritdoc />
    public Expression<Func<T, bool>>? Criteria { get; private set; }

    /// <inheritdoc />
    public List<Expression<Func<T, object>>> Includes { get; }

    /// <inheritdoc />
    public List<string> IncludeStrings { get; }

    /// <inheritdoc />
    public List<(Expression<Func<T, object>> OrderBy, bool IsDescending)> OrderByExpressions { get; }

    /// <inheritdoc />
    public int? Skip { get; private set; }

    /// <inheritdoc />
    public int? Take { get; private set; }

    /// <inheritdoc />
    public bool IsTrackingEnabled { get; private set; }

    /// <inheritdoc />
    public bool IsSplitQuery { get; private set; }

    /// <inheritdoc />
    public bool IsIgnoreQueryFilters { get; private set; }

    /// <inheritdoc />
    public bool IsIgnoreAutoIncludes { get; private set; }

    /// <summary>
    /// Sets the criteria for the specification.
    /// </summary>
    /// <param name="criteria">The criteria expression.</param>
    /// <returns>The specification instance.</returns>
    protected virtual ISpecification<T> SetCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderByExpressions.Add((orderByExpression, false));
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByExpressions.Add((orderByDescendingExpression, true));
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> EnableTracking(bool isTrackingEnabled = true)
    {
        IsTrackingEnabled = isTrackingEnabled;
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> EnableSplitQuery(bool isSplitQuery = true)
    {
        IsSplitQuery = isSplitQuery;
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> IgnoreQueryFilters(bool isIgnoreQueryFilters = true)
    {
        IsIgnoreQueryFilters = isIgnoreQueryFilters;
        return this;
    }

    /// <inheritdoc />
    public virtual ISpecification<T> IgnoreAutoIncludes(bool isIgnoreAutoIncludes = true)
    {
        IsIgnoreAutoIncludes = isIgnoreAutoIncludes;
        return this;
    }
}