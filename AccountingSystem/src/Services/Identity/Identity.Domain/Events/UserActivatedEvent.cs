using BuildingBlocks.Common.Events;

namespace Identity.Domain.Events;

/// <summary>
/// Event raised when a user is activated.
/// </summary>
public class UserActivatedEvent : DomainEvent
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
    /// Initializes a new instance of the <see cref="UserActivatedEvent"/> class.
    /// </summary>
    public UserActivatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}
