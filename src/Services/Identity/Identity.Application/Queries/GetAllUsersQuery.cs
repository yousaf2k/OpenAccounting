using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get all users.
/// </summary>
public class GetAllUsersQuery : IRequest<List<UserDto>>
{
    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
