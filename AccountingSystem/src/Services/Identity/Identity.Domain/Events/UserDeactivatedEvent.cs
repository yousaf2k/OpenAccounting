using BuildingBlocks.Common.Events;

namespace Identity.Domain.Events;

/// <summary>
/// Event raised when a user is deactivated.
/// </summary>
public class UserDeactivatedEvent : DomainEvent
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
    /// Initializes a new instance of the <see cref="UserDeactivatedEvent"/> class.
    /// </summary>
    public UserDeactivatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}
