using BuildingBlocks.EventBus.Abstractions;

namespace Identity.Application.IntegrationEvents;

/// <summary>
/// Integration event for user registration.
/// </summary>
public class UserRegisteredIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisteredIntegrationEvent"/> class.
    /// </summary>
    public UserRegisteredIntegrationEvent(Guid userId, string? email, string? userName, string? firstName, string? lastName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }
}
