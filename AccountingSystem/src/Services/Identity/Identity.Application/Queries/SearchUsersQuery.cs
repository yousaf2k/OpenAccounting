using MediatR;

namespace Identity.Application.Queries;

/// <summary>
/// Query to search users.
/// </summary>
public class SearchUsersQuery : IRequest<List<string>>
{
    /// <summary>
    /// Gets or sets the search term.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchUsersQuery"/> class.
    /// </summary>
    public SearchUsersQuery(string? searchTerm = null)
    {
        SearchTerm = searchTerm;
    }
}
