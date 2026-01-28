using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to update an existing role (Comment 2).
/// </summary>
public class UpdateRoleCommand : IRequest<RoleDto>
{
    /// <summary>
    /// Role ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Updated role name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Updated role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Updated role permissions as JSON.
    /// </summary>
    public string? Permissions { get; set; }
}
