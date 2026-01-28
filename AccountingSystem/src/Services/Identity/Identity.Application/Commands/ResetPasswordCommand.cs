using MediatR;

namespace Identity.Application.Commands;

/// <summary>
/// Command to reset a password.
/// </summary>
public class ResetPasswordCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the reset token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string? NewPassword { get; set; }
}
