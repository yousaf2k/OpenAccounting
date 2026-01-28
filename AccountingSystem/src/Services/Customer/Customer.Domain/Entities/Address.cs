using BuildingBlocks.Common.Abstractions;
using Customer.Domain.Enums;

namespace Customer.Domain.Entities;

/// <summary>
/// Address entity.
/// </summary>
public class Address : Entity<Guid>, IAuditableEntity, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the address type.
    /// </summary>
    public AddressType AddressType { get; set; }

    /// <summary>
    /// Gets or sets the first street line.
    /// </summary>
    public string Street1 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the second street line.
    /// </summary>
    public string? Street2 { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state or province.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this is the primary address.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets the associated customer ID.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the associated vendor ID.
    /// </summary>
    public Guid? VendorId { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the last modification timestamp.
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who last modified the entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the soft delete timestamp.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is soft deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
