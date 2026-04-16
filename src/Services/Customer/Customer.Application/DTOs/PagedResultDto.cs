namespace Customer.Application.DTOs;

/// <summary>
/// DTO for paginated results.
/// </summary>
public class PagedResult<T>
{
    /// <summary>Gets or sets the items.</summary>
    public List<T> Items { get; set; } = [];

    /// <summary>Gets or sets the total count.</summary>
    public int TotalCount { get; set; }

    /// <summary>Gets or sets the page number.</summary>
    public int PageNumber { get; set; }

    /// <summary>Gets or sets the page size.</summary>
    public int PageSize { get; set; }

    /// <summary>Gets the total pages.</summary>
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    /// <summary>Gets a value indicating whether there is a next page.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>Gets a value indicating whether there is a previous page.</summary>
    public bool HasPreviousPage => PageNumber > 1;
}
