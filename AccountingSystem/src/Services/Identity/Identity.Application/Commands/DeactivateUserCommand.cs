using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to deactivate a user.
/// </summary>
public class DeactivateUserCommand : IRequest<UserDto>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }
}
