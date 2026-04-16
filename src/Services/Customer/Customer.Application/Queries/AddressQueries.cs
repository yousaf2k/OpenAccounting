using MediatR;
using Customer.Application.DTOs;

namespace Customer.Application.Queries;

/// <summary>
/// Query to get an address by ID.
/// </summary>
public class GetAddressByIdQuery : IRequest<AddressDto?>
{
    /// <summary>Gets or sets the address ID.</summary>
    public Guid AddressId { get; set; }
}

/// <summary>
/// Query to get all addresses for a customer.
/// </summary>
public class GetCustomerAddressesQuery : IRequest<List<AddressDto>>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }
}
