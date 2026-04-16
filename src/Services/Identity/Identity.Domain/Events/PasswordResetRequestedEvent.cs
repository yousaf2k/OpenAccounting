using BuildingBlocks.Common.Events;

namespace Identity.Domain.Events;

/// <summary>
/// Event raised when a password reset is requested.
/// </summary>
public class PasswordResetRequestedEvent : DomainEvent
{
    /// <summary>
    /// Gets the user ID.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the user email.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the password reset token.
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetRequestedEvent"/> class.
    /// </summary>
    public PasswordResetRequestedEvent(Guid userId, string email, string token)
    {
        UserId = userId;
        Email = email;
        Token = token;
    }
}
