using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Customer.Application.Commands;
using Customer.Application.Queries;
using Customer.Application.DTOs;
using Serilog;

namespace Customer.API.Controllers;

/// <summary>
/// API controller for managing vendors.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = Log.ForContext<VendorsController>();
    }

    /// <summary>
    /// Gets all vendors with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<VendorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<VendorDto>>> GetAllVendors(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting all vendors - Page: {PageNumber}, Size: {PageSize}, SearchTerm: {SearchTerm}", pageNumber, pageSize, searchTerm);

        var query = new GetAllVendorsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a vendor by ID.
    /// </summary>
    [HttpGet("{vendorId}")]
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VendorDto>> GetVendorById(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting vendor by ID: {VendorId}", vendorId);

        var query = new GetVendorByIdQuery { VendorId = vendorId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", vendorId);
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a vendor by email.
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VendorDto>> GetVendorByEmail(
        string email,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting vendor by email: {Email}", email);

        var query = new GetVendorByEmailQuery { Email = email };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.Warning("Vendor not found: {Email}", email);
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Searches for vendors.
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(List<VendorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VendorDto>>> SearchVendors(
        [FromBody] SearchVendorsRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Searching vendors - SearchTerm: {SearchTerm}", request.SearchTerm);

        var query = new SearchVendorsQuery
        {
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new vendor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateVendor(
        [FromBody] CreateVendorDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Creating vendor: {CompanyName}", createDto.CompanyName);

        var command = new CreateVendorCommand
        {
            CompanyName = createDto.CompanyName,
            TaxId = createDto.TaxId,
            Email = createDto.Email,
            Phone = createDto.Phone,
            Website = createDto.Website,
            VendorType = createDto.VendorType,
            PaymentTerms = createDto.PaymentTerms,
            Notes = createDto.Notes
        };

        var vendorId = await _mediator.Send(command, cancellationToken);
        _logger.Information("Vendor created successfully with ID: {VendorId}", vendorId);

        return CreatedAtAction(nameof(GetVendorById), new { vendorId }, vendorId);
    }

    /// <summary>
    /// Updates an existing vendor.
    /// </summary>
    [HttpPut("{vendorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVendor(
        Guid vendorId,
        [FromBody] UpdateVendorDto updateDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Updating vendor: {VendorId}", vendorId);

        var command = new UpdateVendorCommand
        {
            VendorId = vendorId,
            CompanyName = updateDto.CompanyName,
            TaxId = updateDto.TaxId,
            Email = updateDto.Email,
            Phone = updateDto.Phone,
            Website = updateDto.Website,
            Notes = updateDto.Notes
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to update vendor: {VendorId}", vendorId);
            return NotFound();
        }

        _logger.Information("Vendor updated successfully: {VendorId}", vendorId);
        return NoContent();
    }

    /// <summary>
    /// Deletes a vendor.
    /// </summary>
    [HttpDelete("{vendorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVendor(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Deleting vendor: {VendorId}", vendorId);

        var command = new DeleteVendorCommand { VendorId = vendorId };
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to delete vendor: {VendorId}", vendorId);
            return NotFound();
        }

        _logger.Information("Vendor deleted successfully: {VendorId}", vendorId);
        return NoContent();
    }

    /// <summary>
    /// Gets contacts for a vendor.
    /// </summary>
    [HttpGet("{vendorId}/contacts")]
    [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ContactDto>>> GetVendorContacts(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting contacts for vendor: {VendorId}", vendorId);

        var query = new GetVendorContactsQuery { VendorId = vendorId };
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Adds a contact to a vendor.
    /// </summary>
    [HttpPost("{vendorId}/contacts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddContactToVendor(
        Guid vendorId,
        [FromBody] CreateContactDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Adding contact to vendor: {VendorId}", vendorId);

        var command = new AddContactToVendorCommand
        {
            VendorId = vendorId,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            Phone = createDto.Phone,
            Mobile = createDto.Mobile,
            Position = createDto.Position,
            IsPrimary = createDto.IsPrimary
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to add contact to vendor: {VendorId}", vendorId);
            return NotFound();
        }

        _logger.Information("Contact added to vendor successfully: {VendorId}", vendorId);
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Removes a contact from a vendor.
    /// </summary>
    [HttpDelete("{vendorId}/contacts/{contactId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveContactFromVendor(
        Guid vendorId,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Removing contact from vendor: {VendorId}, {ContactId}", vendorId, contactId);

        var command = new RemoveContactFromVendorCommand
        {
            VendorId = vendorId,
            ContactId = contactId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to remove contact from vendor: {VendorId}, {ContactId}", vendorId, contactId);
            return NotFound();
        }

        _logger.Information("Contact removed from vendor successfully");
        return NoContent();
    }

    /// <summary>
    /// Gets addresses for a vendor.
    /// </summary>
    [HttpGet("{vendorId}/addresses")]
    [ProducesResponseType(typeof(List<AddressDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AddressDto>>> GetVendorAddresses(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting addresses for vendor: {VendorId}", vendorId);

        var query = new GetVendorAddressesQuery { VendorId = vendorId };
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Adds an address to a vendor.
    /// </summary>
    [HttpPost("{vendorId}/addresses")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAddressToVendor(
        Guid vendorId,
        [FromBody] CreateAddressDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Adding address to vendor: {VendorId}", vendorId);

        var command = new AddAddressToVendorCommand
        {
            VendorId = vendorId,
            AddressType = createDto.AddressType,
            Street1 = createDto.Street1,
            Street2 = createDto.Street2,
            City = createDto.City,
            State = createDto.State,
            PostalCode = createDto.PostalCode,
            Country = createDto.Country,
            IsPrimary = createDto.IsPrimary
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to add address to vendor: {VendorId}", vendorId);
            return NotFound();
        }

        _logger.Information("Address added to vendor successfully: {VendorId}", vendorId);
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Removes an address from a vendor.
    /// </summary>
    [HttpDelete("{vendorId}/addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAddressFromVendor(
        Guid vendorId,
        Guid addressId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Removing address from vendor: {VendorId}, {AddressId}", vendorId, addressId);

        var command = new RemoveAddressFromVendorCommand
        {
            VendorId = vendorId,
            AddressId = addressId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to remove address from vendor: {VendorId}, {AddressId}", vendorId, addressId);
            return NotFound();
        }

        _logger.Information("Address removed from vendor successfully");
        return NoContent();
    }
}

/// <summary>
/// Request model for searching vendors.
/// </summary>
public class SearchVendorsRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
