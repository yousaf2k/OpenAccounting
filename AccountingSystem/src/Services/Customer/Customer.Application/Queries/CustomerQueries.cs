using MediatR;
using Customer.Application.DTOs;

namespace Customer.Application.Queries;

/// <summary>
/// Query to get a customer by ID.
/// </summary>
public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    /// <summary>Gets or sets the customer ID.</summary>
    public Guid CustomerId { get; set; }
}

/// <summary>
/// Query to get all customers with pagination.
/// </summary>
public class GetAllCustomersQuery : IRequest<PagedResult<CustomerDto>>
{
    /// <summary>Gets or sets the page number.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Gets or sets the page size.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Gets or sets the optional search term.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>Gets or sets the optional customer type filter.</summary>
    public string? CustomerType { get; set; }
}

/// <summary>
/// Query to get a customer by customer number.
/// </summary>
public class GetCustomerByNumberQuery : IRequest<CustomerDto?>
{
    /// <summary>Gets or sets the customer number.</summary>
    public string CustomerNumber { get; set; } = string.Empty;
}

/// <summary>
/// Query to get a customer by email.
/// </summary>
public class GetCustomerByEmailQuery : IRequest<CustomerDto?>
{
    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Query to search customers.
/// </summary>
public class SearchCustomersQuery : IRequest<List<CustomerDto>>
{
    /// <summary>Gets or sets the search term.</summary>
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>Gets or sets the page number.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Gets or sets the page size.</summary>
    public int PageSize { get; set; } = 10;
}
