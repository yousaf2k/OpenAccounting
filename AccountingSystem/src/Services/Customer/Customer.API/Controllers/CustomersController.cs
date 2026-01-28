using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Customer.Application.Commands;
using Customer.Application.Queries;
using Customer.Application.DTOs;
using Serilog;

namespace Customer.API.Controllers;

/// <summary>
/// API controller for managing customers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = Log.ForContext<CustomersController>();
    }

    /// <summary>
    /// Gets all customers with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<CustomerDto>>> GetAllCustomers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting all customers - Page: {PageNumber}, Size: {PageSize}, SearchTerm: {SearchTerm}", pageNumber, pageSize, searchTerm);

        var query = new GetAllCustomersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a customer by ID.
    /// </summary>
    [HttpGet("{customerId}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer by ID: {CustomerId}", customerId);

        var query = new GetCustomerByIdQuery { CustomerId = customerId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", customerId);
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a customer by customer number.
    /// </summary>
    [HttpGet("number/{customerNumber}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerByNumber(
        string customerNumber,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer by number: {CustomerNumber}", customerNumber);

        var query = new GetCustomerByNumberQuery { CustomerNumber = customerNumber };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.Warning("Customer not found: {CustomerNumber}", customerNumber);
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a customer by email.
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(
        string email,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer by email: {Email}", email);

        var query = new GetCustomerByEmailQuery { Email = email };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.Warning("Customer not found: {Email}", email);
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Searches for customers.
    /// </summary>
    [HttpPost("search")]
    [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CustomerDto>>> SearchCustomers(
        [FromBody] SearchCustomersRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Searching customers - SearchTerm: {SearchTerm}", request.SearchTerm);

        var query = new SearchCustomersQuery
        {
            SearchTerm = request.SearchTerm,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateCustomer(
        [FromBody] CreateCustomerDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Creating customer: {CompanyName}", createDto.CompanyName);

        var command = new CreateCustomerCommand
        {
            CustomerNumber = createDto.CustomerNumber,
            CompanyName = createDto.CompanyName,
            TaxId = createDto.TaxId,
            Email = createDto.Email,
            Phone = createDto.Phone,
            Website = createDto.Website,
            CustomerType = createDto.CustomerType,
            PaymentTerms = createDto.PaymentTerms,
            CreditLimit = createDto.CreditLimit,
            Notes = createDto.Notes
        };

        var customerId = await _mediator.Send(command, cancellationToken);
        _logger.Information("Customer created successfully with ID: {CustomerId}", customerId);

        return CreatedAtAction(nameof(GetCustomerById), new { customerId }, customerId);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    [HttpPut("{customerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCustomer(
        Guid customerId,
        [FromBody] UpdateCustomerDto updateDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Updating customer: {CustomerId}", customerId);

        var command = new UpdateCustomerCommand
        {
            CustomerId = customerId,
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
            _logger.Warning("Failed to update customer: {CustomerId}", customerId);
            return NotFound();
        }

        _logger.Information("Customer updated successfully: {CustomerId}", customerId);
        return NoContent();
    }

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    [HttpDelete("{customerId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Deleting customer: {CustomerId}", customerId);

        var command = new DeleteCustomerCommand { CustomerId = customerId };
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to delete customer: {CustomerId}", customerId);
            return NotFound();
        }

        _logger.Information("Customer deleted successfully: {CustomerId}", customerId);
        return NoContent();
    }

    /// <summary>
    /// Gets contacts for a customer.
    /// </summary>
    [HttpGet("{customerId}/contacts")]
    [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ContactDto>>> GetCustomerContacts(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting contacts for customer: {CustomerId}", customerId);

        var query = new GetCustomerContactsQuery { CustomerId = customerId };
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Adds a contact to a customer.
    /// </summary>
    [HttpPost("{customerId}/contacts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddContactToCustomer(
        Guid customerId,
        [FromBody] CreateContactDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Adding contact to customer: {CustomerId}", customerId);

        var command = new AddContactToCustomerCommand
        {
            CustomerId = customerId,
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
            _logger.Warning("Failed to add contact to customer: {CustomerId}", customerId);
            return NotFound();
        }

        _logger.Information("Contact added to customer successfully: {CustomerId}", customerId);
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Removes a contact from a customer.
    /// </summary>
    [HttpDelete("{customerId}/contacts/{contactId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveContactFromCustomer(
        Guid customerId,
        Guid contactId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Removing contact from customer: {CustomerId}, {ContactId}", customerId, contactId);

        var command = new RemoveContactFromCustomerCommand
        {
            CustomerId = customerId,
            ContactId = contactId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to remove contact from customer: {CustomerId}, {ContactId}", customerId, contactId);
            return NotFound();
        }

        _logger.Information("Contact removed from customer successfully");
        return NoContent();
    }

    /// <summary>
    /// Gets addresses for a customer.
    /// </summary>
    [HttpGet("{customerId}/addresses")]
    [ProducesResponseType(typeof(List<AddressDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AddressDto>>> GetCustomerAddresses(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting addresses for customer: {CustomerId}", customerId);

        var query = new GetCustomerAddressesQuery { CustomerId = customerId };
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Adds an address to a customer.
    /// </summary>
    [HttpPost("{customerId}/addresses")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAddressToCustomer(
        Guid customerId,
        [FromBody] CreateAddressDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Adding address to customer: {CustomerId}", customerId);

        var command = new AddAddressToCustomerCommand
        {
            CustomerId = customerId,
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
            _logger.Warning("Failed to add address to customer: {CustomerId}", customerId);
            return NotFound();
        }

        _logger.Information("Address added to customer successfully: {CustomerId}", customerId);
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Removes an address from a customer.
    /// </summary>
    [HttpDelete("{customerId}/addresses/{addressId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAddressFromCustomer(
        Guid customerId,
        Guid addressId,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Removing address from customer: {CustomerId}, {AddressId}", customerId, addressId);

        var command = new RemoveAddressFromCustomerCommand
        {
            CustomerId = customerId,
            AddressId = addressId
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to remove address from customer: {CustomerId}, {AddressId}", customerId, addressId);
            return NotFound();
        }

        _logger.Information("Address removed from customer successfully");
        return NoContent();
    }

    /// <summary>
    /// Updates the credit limit for a customer.
    /// </summary>
    [HttpPatch("{customerId}/credit-limit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCreditLimit(
        Guid customerId,
        [FromBody] UpdateCreditLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Updating credit limit for customer: {CustomerId}, NewLimit: {CreditLimit}", customerId, request.NewCreditLimit);

        var command = new UpdateCreditLimitCommand
        {
            CustomerId = customerId,
            NewCreditLimit = request.NewCreditLimit
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
        {
            _logger.Warning("Failed to update credit limit for customer: {CustomerId}", customerId);
            return NotFound();
        }

        _logger.Information("Credit limit updated successfully for customer: {CustomerId}", customerId);
        return NoContent();
    }
}

/// <summary>
/// Request model for searching customers.
/// </summary>
public class SearchCustomersRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Request model for updating credit limit.
/// </summary>
public class UpdateCreditLimitRequest
{
    public decimal NewCreditLimit { get; set; }
}
