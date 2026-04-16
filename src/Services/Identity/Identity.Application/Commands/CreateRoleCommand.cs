using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to create a new role (Comment 2).
/// </summary>
public class CreateRoleCommand : IRequest<RoleDto>
{
    /// <summary>
    /// Role name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Role permissions as JSON.
    /// </summary>
    public string? Permissions { get; set; }
}
