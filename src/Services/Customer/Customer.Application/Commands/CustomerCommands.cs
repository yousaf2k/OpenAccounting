using MediatR;
using Customer.Domain.Enums;

namespace Customer.Application.Commands;

/// <summary>
/// Command to create a new customer.
/// </summary>
public class CreateCustomerCommand : IRequest<Guid>
{
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
    public CustomerType CustomerType { get; set; }

    /// <summary>Gets or sets the payment terms.</summary>
    public PaymentTerms PaymentTerms { get; set; }

    /// <summary>Gets or sets the credit limit.</summary>
    public decimal CreditLimit { get; set; }

    /// <summary>Gets or sets additional notes.</summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Command to update an existing customer.
/// </summary>
public class UpdateCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

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

    /// <summary>Gets or sets additional notes.</summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Command to delete a customer.
/// </summary>
public class DeleteCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }
}

/// <summary>
/// Command to add a contact to a customer.
/// </summary>
public class AddContactToCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

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
/// Command to remove a contact from a customer.
/// </summary>
public class RemoveContactFromCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the contact ID.</summary>
    public Guid ContactId { get; set; }
}

/// <summary>
/// Command to add an address to a customer.
/// </summary>
public class AddAddressToCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the address type.</summary>
    public string AddressType { get; set; } = string.Empty;

    /// <summary>Gets or sets the street address line 1.</summary>
    public string Street1 { get; set; } = string.Empty;

    /// <summary>Gets or sets the street address line 2.</summary>
    public string? Street2 { get; set; }

    /// <summary>Gets or sets the city.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Gets or sets the state.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Gets or sets the postal code.</summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Gets or sets the country.</summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>Gets or sets whether this is the primary address.</summary>
    public bool IsPrimary { get; set; }
}

/// <summary>
/// Command to remove an address from a customer.
/// </summary>
public class RemoveAddressFromCustomerCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the address ID.</summary>
    public Guid AddressId { get; set; }
}

/// <summary>
/// Command to update a customer's credit limit.
/// </summary>
public class UpdateCreditLimitCommand : IRequest<bool>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }

    /// <summary>Gets or sets the new credit limit.</summary>
    public decimal NewCreditLimit { get; set; }
}
