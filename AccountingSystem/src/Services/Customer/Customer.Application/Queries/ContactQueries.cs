using MediatR;
using Customer.Application.DTOs;

namespace Customer.Application.Queries;

/// <summary>
/// Query to get a contact by ID.
/// </summary>
public class GetContactByIdQuery : IRequest<ContactDto?>
{
    /// <summary>Gets or sets the contact ID.</summary>
    public Guid ContactId { get; set; }
}

/// <summary>
/// Query to get all contacts for a customer.
/// </summary>
public class GetCustomerContactsQuery : IRequest<List<ContactDto>>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }
}
