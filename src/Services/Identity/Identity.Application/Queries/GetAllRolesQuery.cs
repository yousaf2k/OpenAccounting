using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get all roles (Comment 2).
/// </summary>
public class GetAllRolesQuery : IRequest<List<RoleDto>>
{
    /// <summary>
    /// Page number for pagination.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size for pagination.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
