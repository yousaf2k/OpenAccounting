using BuildingBlocks.Common.Events;

namespace Identity.Domain.Events;

/// <summary>
/// Event raised when a user is registered.
/// </summary>
public class UserRegisteredEvent : DomainEvent
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
    /// Gets the user name.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisteredEvent"/> class.
    /// </summary>
    public UserRegisteredEvent(Guid userId, string email, string userName, string firstName, string lastName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }
}
