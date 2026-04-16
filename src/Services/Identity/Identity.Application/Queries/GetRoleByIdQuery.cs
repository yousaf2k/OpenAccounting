using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get a role by ID (Comment 2).
/// </summary>
public class GetRoleByIdQuery : IRequest<RoleDto?>
{
    /// <summary>
    /// Role ID to retrieve.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRoleByIdQuery"/> class.
    /// </summary>
    public GetRoleByIdQuery(Guid id)
    {
        Id = id;
    }
}
