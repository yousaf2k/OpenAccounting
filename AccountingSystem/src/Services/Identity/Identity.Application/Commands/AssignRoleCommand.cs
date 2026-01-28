using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to assign a role to a user.
/// </summary>
public class AssignRoleCommand : IRequest<UserDto>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}
