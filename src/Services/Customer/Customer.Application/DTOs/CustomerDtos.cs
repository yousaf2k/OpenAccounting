namespace Customer.Application.DTOs;

/// <summary>
/// DTO for customer data.
/// </summary>
public class CustomerDto
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid Id { get; set; }

    /// <summary>Gets or sets the customer number.</summary>
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the tax ID.</summary>
    public string? TaxId { get; set; }

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the website URL.</summary>
    public string? Website { get; set; }

    /// <summary>Gets or sets the customer type.</summary>
    public string CustomerType { get; set; } = string.Empty;

    /// <summary>Gets or sets the payment terms.</summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>Gets or sets the credit limit.</summary>
    public decimal CreditLimit { get; set; }

    /// <summary>Gets or sets additional notes.</summary>
    public string? Notes { get; set; }

    /// <summary>Gets or sets the contacts.</summary>
    public List<ContactDto> Contacts { get; set; } = [];

    /// <summary>Gets or sets the addresses.</summary>
    public List<AddressDto> Addresses { get; set; } = [];

    /// <summary>Gets or sets the creation timestamp.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the last modification timestamp.</summary>
    public DateTime? LastModifiedAt { get; set; }
}

/// <summary>
/// DTO for creating a customer.
/// </summary>
public class CreateCustomerDto
{
    /// <summary>Gets or sets the company name.</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Gets or sets the tax ID.</summary>
    public string? TaxId { get; set; }

    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the website URL.</summary>
    public string? Website { get; set; }

    /// <summary>Gets or sets the customer type.</summary>
    public string CustomerType { get; set; } = string.Empty;

    /// <summary>Gets or sets the payment terms.</summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>Gets or sets the credit limit.</summary>
    public decimal CreditLimit { get; set; }

    /// <summary>Gets or sets additional notes.</summary>
    public string? Notes { get; set; }
}

/// <summary>
/// DTO for updating a customer.
/// </summary>
public class UpdateCustomerDto
{
    /// <summary>Gets or sets the company name.</summary>
    public string? CompanyName { get; set; }

    /// <summary>Gets or sets the tax ID.</summary>
    public string? TaxId { get; set; }

    /// <summary>Gets or sets the email address.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the phone number.</summary>
    public string? Phone { get; set; }

    /// <summary>Gets or sets the website URL.</summary>
    public string? Website { get; set; }

    /// <summary>Gets or sets the customer type.</summary>
    public string? CustomerType { get; set; }

    /// <summary>Gets or sets the payment terms.</summary>
    public string? PaymentTerms { get; set; }

    /// <summary>Gets or sets the credit limit.</summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>Gets or sets additional notes.</summary>
    public string? Notes { get; set; }
}
