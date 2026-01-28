using BuildingBlocks.Common.Events;

namespace Identity.Domain.Events;

/// <summary>
/// Event raised when a user's role is changed.
/// </summary>
public class UserRoleChangedEvent : DomainEvent
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
    /// Gets the role name.
    /// </summary>
    public string RoleName { get; }

    /// <summary>
    /// Gets a value indicating whether the role was added.
    /// </summary>
    public bool IsAdded { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleChangedEvent"/> class.
    /// </summary>
    public UserRoleChangedEvent(Guid userId, string email, string roleName, bool isAdded)
    {
        UserId = userId;
        Email = email;
        RoleName = roleName;
        IsAdded = isAdded;
    }
}
