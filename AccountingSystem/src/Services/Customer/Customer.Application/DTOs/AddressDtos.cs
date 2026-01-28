namespace Customer.Application.DTOs;

/// <summary>
/// DTO for address data.
/// </summary>
public class AddressDto
{
    /// <summary>Gets or sets the address ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the address type.</summary>
    public string AddressType { get; set; } = string.Empty;

    /// <summary>Gets or sets the first street line.</summary>
    public string Street1 { get; set; } = string.Empty;

    /// <summary>Gets or sets the second street line.</summary>
    public string? Street2 { get; set; }

    /// <summary>Gets or sets the city.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Gets or sets the state or province.</summary>
    public string? State { get; set; }

    /// <summary>Gets or sets the postal code.</summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the country.</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>Gets or sets whether this is the primary address.</summary>
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO for creating an address.
/// </summary>
public class CreateAddressDto
{
    /// <summary>Gets or sets the address type.</summary>
    public string AddressType { get; set; } = string.Empty;

    /// <summary>Gets or sets the first street line.</summary>
    public string Street1 { get; set; } = string.Empty;

    /// <summary>Gets or sets the second street line.</summary>
    public string? Street2 { get; set; }

    /// <summary>Gets or sets the city.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Gets or sets the state or province.</summary>
    public string? State { get; set; }

    /// <summary>Gets or sets the postal code.</summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the country.</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>Gets or sets whether this is the primary address.</summary>
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO for updating an address.
/// </summary>
public class UpdateAddressDto
{
    /// <summary>Gets or sets the address type.</summary>
    public string? AddressType { get; set; }

    /// <summary>Gets or sets the first street line.</summary>
    public string? Street1 { get; set; }

    /// <summary>Gets or sets the second street line.</summary>
    public string? Street2 { get; set; }

    /// <summary>Gets or sets the city.</summary>
    public string? City { get; set; }

    /// <summary>Gets or sets the state or province.</summary>
    public string? State { get; set; }

    /// <summary>Gets or sets the postal code.</summary>
    public string? PostalCode { get; set; }

    /// <summary>Gets or sets the country.</summary>
    public string? Country { get; set; }

    /// <summary>Gets or sets whether this is the primary address.</summary>
    public bool? IsPrimary { get; set; }
}
