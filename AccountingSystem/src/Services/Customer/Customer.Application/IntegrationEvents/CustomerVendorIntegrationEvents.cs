using BuildingBlocks.EventBus;

namespace Customer.Application.IntegrationEvents;

/// <summary>
/// Integration event published when a customer is created.
/// </summary>
public class CustomerCreatedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Gets or sets the tax ID.</summary>
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Gets or sets the customer type.</summary>
    public string CustomerType { get; set; } = string.Empty;
}

/// <summary>
/// Integration event published when a customer is updated.
/// </summary>
public class CustomerUpdatedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Gets or sets the credit limit.</summary>
    public decimal CreditLimit { get; set; }

    /// <summary>Gets or sets the timestamp of the update.</summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Integration event published when a customer is deleted.
/// </summary>
public class CustomerDeletedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the timestamp of the deletion.</summary>
    public DateTime DeletedAt { get; set; }
}

/// <summary>
/// Integration event published when a vendor is created.
/// </summary>
public class VendorCreatedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Gets or sets the tax ID.</summary>
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Gets or sets the vendor type.</summary>
    public string VendorType { get; set; } = string.Empty;
}

/// <summary>
/// Integration event published when a vendor is updated.
/// </summary>
public class VendorUpdatedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Gets or sets the timestamp of the update.</summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Integration event published when a vendor is deleted.
/// </summary>
public class VendorDeletedIntegrationEvent : IntegrationEvent
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the timestamp of the deletion.</summary>
    public DateTime DeletedAt { get; set; }
}
