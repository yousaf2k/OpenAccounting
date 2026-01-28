using MediatR;

namespace Identity.Application.Commands;

/// <summary>
/// Command to request a password reset.
/// </summary>
public class RequestPasswordResetCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string? Email { get; set; }
}
