using BuildingBlocks.Common.Abstractions;
using Customer.Domain.Enums;

namespace Customer.Domain.Entities;

/// <summary>
/// Customer aggregate root entity.
/// </summary>
public class Customer : AggregateRoot<Guid>, IAuditableEntity, ISoftDeletable
{
    /// <summary>
    /// Gets or sets the unique customer number.
    /// </summary>
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tax ID.
    /// </summary>
    public string? TaxId { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gets or sets the website URL.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets the customer type.
    /// </summary>
    public CustomerType CustomerType { get; set; }

    /// <summary>
    /// Gets or sets the payment terms.
    /// </summary>
    public PaymentTerms PaymentTerms { get; set; }

    /// <summary>
    /// Gets or sets the credit limit.
    /// </summary>
    public decimal CreditLimit { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    public string? Notes { get; set; }

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

    /// <summary>
    /// Gets or sets the collection of contacts.
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Gets or sets the collection of addresses.
    /// </summary>
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Adds a contact to the customer.
    /// </summary>
    public void AddContact(Contact contact)
    {
        contact.CustomerId = Id;
        contact.VendorId = null;
        Contacts.Add(contact);
    }

    /// <summary>
    /// Removes a contact from the customer.
    /// </summary>
    public void RemoveContact(Contact contact)
    {
        Contacts.Remove(contact);
    }

    /// <summary>
    /// Adds an address to the customer.
    /// </summary>
    public void AddAddress(Address address)
    {
        address.CustomerId = Id;
        address.VendorId = null;
        Addresses.Add(address);
    }

    /// <summary>
    /// Removes an address from the customer.
    /// </summary>
    public void RemoveAddress(Address address)
    {
        Addresses.Remove(address);
    }

    /// <summary>
    /// Updates the customer's credit limit.
    /// </summary>
    public void UpdateCreditLimit(decimal newLimit)
    {
        if (newLimit < 0)
        {
            throw new ArgumentException("Credit limit cannot be negative", nameof(newLimit));
        }

        CreditLimit = newLimit;
    }
}
