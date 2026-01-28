using MediatR;

namespace Customer.Domain.Events;

/// <summary>
/// Domain event published when a customer is created.
/// </summary>
public class CustomerCreatedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerCreatedDomainEvent"/> class.
    /// </summary>
    public CustomerCreatedDomainEvent(Guid customerId, string companyName, string email)
    {
        CustomerId = customerId;
        CompanyName = companyName;
        Email = email;
    }

    /// <summary>
    /// Gets the customer ID.
    /// </summary>
    public Guid CustomerId { get; }

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
/// Domain event published when a customer is updated.
/// </summary>
public class CustomerUpdatedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerUpdatedDomainEvent"/> class.
    /// </summary>
    public CustomerUpdatedDomainEvent(Guid customerId, string companyName)
    {
        CustomerId = customerId;
        CompanyName = companyName;
    }

    /// <summary>
    /// Gets the customer ID.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the company name.
    /// </summary>
    public string CompanyName { get; }
}

/// <summary>
/// Domain event published when a customer is deleted.
/// </summary>
public class CustomerDeletedDomainEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerDeletedDomainEvent"/> class.
    /// </summary>
    public CustomerDeletedDomainEvent(Guid customerId)
    {
        CustomerId = customerId;
    }

    /// <summary>
    /// Gets the customer ID.
    /// </summary>
    public Guid CustomerId { get; }
}
