using MediatR;

namespace Customer.Domain.Events;

/// <summary>
/// Domain event published when a vendor is created.
/// </summary>
public class VendorCreatedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VendorCreatedDomainEvent"/> class.
    /// </summary>
    public VendorCreatedDomainEvent(Guid vendorId, string companyName, string email)
    {
        VendorId = vendorId;
        CompanyName = companyName;
        Email = email;
    }

    /// <summary>
    /// Gets the vendor ID.
    /// </summary>
    public Guid VendorId { get; }

    /// <summary>
    /// Gets the company name.
    /// </summary>
    public string CompanyName { get; }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string Email { get; }
}

/// <summary>
/// Domain event published when a vendor is updated.
/// </summary>
public class VendorUpdatedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VendorUpdatedDomainEvent"/> class.
    /// </summary>
    public VendorUpdatedDomainEvent(Guid vendorId, string companyName)
    {
        VendorId = vendorId;
        CompanyName = companyName;
    }

    /// <summary>
    /// Gets the vendor ID.
    /// </summary>
    public Guid VendorId { get; }

    /// <summary>
    /// Gets the company name.
    /// </summary>
    public string CompanyName { get; }
}

/// <summary>
/// Domain event published when a vendor is deleted.
/// </summary>
public class VendorDeletedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VendorDeletedDomainEvent"/> class.
    /// </summary>
    public VendorDeletedDomainEvent(Guid vendorId)
    {
        VendorId = vendorId;
    }

    /// <summary>
    /// Gets the vendor ID.
    /// </summary>
    public Guid VendorId { get; }
}
