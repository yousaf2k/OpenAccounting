using MediatR;

namespace Identity.Application.Commands;

/// <summary>
/// Command to change a user's password.
/// </summary>
public class ChangePasswordCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the old password.
    /// </summary>
    public string? OldPassword { get; set; }

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string? NewPassword { get; set; }
}
