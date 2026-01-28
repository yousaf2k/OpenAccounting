namespace Customer.Application.DTOs;

/// <summary>
/// DTO for contact data.
/// </summary>
public class ContactDto
{
    /// <summary>Gets or sets the contact ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the mobile number.</summary>
    public string? Mobile { get; set; }

    /// <summary>Gets or sets the position.</summary>
    public string? Position { get; set; }

    /// <summary>Gets or sets whether this is the primary contact.</summary>
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO for creating a contact.
/// </summary>
public class CreateContactDto
{
    /// <summary>Gets or sets the first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the mobile number.</summary>
    public string? Mobile { get; set; }

    /// <summary>Gets or sets the position.</summary>
    public string? Position { get; set; }

    /// <summary>Gets or sets whether this is the primary contact.</summary>
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO for updating a contact.
/// </summary>
public class UpdateContactDto
{
    /// <summary>Gets or sets the first name.</summary>
    public string? FirstName { get; set; }

    /// <summary>Gets or sets the last name.</summary>
    public string? LastName { get; set; }

    /// <summary>Gets or sets the email address.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the mobile number.</summary>
    public string? Mobile { get; set; }

    /// <summary>Gets or sets the position.</summary>
    public string? Position { get; set; }

    /// <summary>Gets or sets whether this is the primary contact.</summary>
    public bool? IsPrimary { get; set; }
}
