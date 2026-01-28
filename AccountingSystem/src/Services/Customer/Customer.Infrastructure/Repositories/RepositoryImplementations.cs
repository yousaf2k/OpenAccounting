using Microsoft.EntityFrameworkCore;
using Customer.Domain.Entities;
using Customer.Infrastructure.Data;
using BuildingBlocks.Infrastructure;
using Serilog;

namespace Customer.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Customer aggregate.
/// </summary>
public class CustomerRepository : Repository<Customer, Guid>, ICustomerRepository
{
    private readonly CustomerDbContext _context;
    private readonly ILogger _logger;

    public CustomerRepository(CustomerDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = Log.ForContext<CustomerRepository>();
    }

    public async Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer by customer number: {CustomerNumber}", customerNumber);
        return await _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer by email: {Email}", email);
        return await _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<List<Customer>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customers paged - Page: {PageNumber}, Size: {PageSize}, SearchTerm: {SearchTerm}", pageNumber, pageSize, searchTerm);

        var query = _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c =>
                c.CompanyName.Contains(searchTerm) ||
                c.CustomerNumber.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));
        }

        var skip = (pageNumber - 1) * pageSize;
        return await query
            .OrderBy(c => c.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting customer count - SearchTerm: {SearchTerm}", searchTerm);

        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(c =>
                c.CompanyName.Contains(searchTerm) ||
                c.CustomerNumber.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<Customer>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        _logger.Information("Searching customers - SearchTerm: {SearchTerm}, Page: {PageNumber}, Size: {PageSize}", searchTerm, pageNumber, pageSize);

        var query = _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Where(c =>
                c.CompanyName.Contains(searchTerm) ||
                c.CustomerNumber.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Phone.Contains(searchTerm));

        var skip = (pageNumber - 1) * pageSize;
        return await query
            .OrderBy(c => c.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

/// <summary>
/// Repository implementation for Vendor aggregate.
/// </summary>
public class VendorRepository : Repository<Vendor, Guid>, IVendorRepository
{
    private readonly CustomerDbContext _context;
    private readonly ILogger _logger;

    public VendorRepository(CustomerDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = Log.ForContext<VendorRepository>();
    }

    public async Task<Vendor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting vendor by email: {Email}", email);
        return await _context.Vendors
            .Include(v => v.Contacts)
            .Include(v => v.Addresses)
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);
    }

    public async Task<List<Vendor>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting vendors paged - Page: {PageNumber}, Size: {PageSize}, SearchTerm: {SearchTerm}", pageNumber, pageSize, searchTerm);

        var query = _context.Vendors
            .Include(v => v.Contacts)
            .Include(v => v.Addresses)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(v =>
                v.CompanyName.Contains(searchTerm) ||
                v.Email.Contains(searchTerm) ||
                v.Phone.Contains(searchTerm));
        }

        var skip = (pageNumber - 1) * pageSize;
        return await query
            .OrderBy(v => v.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting vendor count - SearchTerm: {SearchTerm}", searchTerm);

        var query = _context.Vendors.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(v =>
                v.CompanyName.Contains(searchTerm) ||
                v.Email.Contains(searchTerm) ||
                v.Phone.Contains(searchTerm));
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<Vendor>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        _logger.Information("Searching vendors - SearchTerm: {SearchTerm}, Page: {PageNumber}, Size: {PageSize}", searchTerm, pageNumber, pageSize);

        var query = _context.Vendors
            .Include(v => v.Contacts)
            .Include(v => v.Addresses)
            .Where(v =>
                v.CompanyName.Contains(searchTerm) ||
                v.Email.Contains(searchTerm) ||
                v.Phone.Contains(searchTerm));

        var skip = (pageNumber - 1) * pageSize;
        return await query
            .OrderBy(v => v.CompanyName)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Vendor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Vendors
            .Include(v => v.Contacts)
            .Include(v => v.Addresses)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }
}

/// <summary>
/// Repository implementation for Contact entity.
/// </summary>
public class ContactRepository : Repository<Contact, Guid>, IContactRepository
{
    private readonly CustomerDbContext _context;
    private readonly ILogger _logger;

    public ContactRepository(CustomerDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = Log.ForContext<ContactRepository>();
    }

    public async Task<List<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting contacts by customer ID: {CustomerId}", customerId);
        return await _context.Contacts
            .Where(c => c.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Contact>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting contacts by vendor ID: {VendorId}", vendorId);
        return await _context.Contacts
            .Where(c => c.VendorId == vendorId)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for Address entity.
/// </summary>
public class AddressRepository : Repository<Address, Guid>, IAddressRepository
{
    private readonly CustomerDbContext _context;
    private readonly ILogger _logger;

    public AddressRepository(CustomerDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = Log.ForContext<AddressRepository>();
    }

    public async Task<List<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting addresses by customer ID: {CustomerId}", customerId);
        return await _context.Addresses
            .Where(a => a.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Address>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default)
    {
        _logger.Information("Getting addresses by vendor ID: {VendorId}", vendorId);
        return await _context.Addresses
            .Where(a => a.VendorId == vendorId)
            .ToListAsync(cancellationToken);
    }
}
