using MediatR;
using Customer.Application.DTOs;

namespace Customer.Application.Queries;

/// <summary>
/// Query to get a vendor by ID.
/// </summary>
public class GetVendorByIdQuery : IRequest<VendorDto?>
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }
}

/// <summary>
/// Query to get all vendors with pagination.
/// </summary>
public class GetAllVendorsQuery : IRequest<PagedResult<VendorDto>>
{
    /// <summary>Gets or sets the page number.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Gets or sets the page size.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Gets or sets the optional search term.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>Gets or sets the optional vendor type filter.</summary>
    public string? VendorType { get; set; }
}

/// <summary>
/// Query to get a vendor by email.
/// </summary>
public class GetVendorByEmailQuery : IRequest<VendorDto?>
{
    /// <summary>Gets or sets the email address.</summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Query to search vendors.
/// </summary>
public class SearchVendorsQuery : IRequest<List<VendorDto>>
{
    /// <summary>Gets or sets the search term.</summary>
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>Gets or sets the page number.</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Gets or sets the page size.</summary>
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Query to get vendor contacts.
/// </summary>
public class GetVendorContactsQuery : IRequest<List<ContactDto>>
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }
}

/// <summary>
/// Query to get vendor addresses.
/// </summary>
public class GetVendorAddressesQuery : IRequest<List<AddressDto>>
{
    /// <summary>Gets or sets the vendor ID.</summary>
    public Guid VendorId { get; set; }
}
