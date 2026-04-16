using Customer.Domain.Entities;
using BuildingBlocks.Infrastructure;

namespace Customer.Infrastructure.Repositories;

/// <summary>
/// Repository interface for Customer aggregate.
/// </summary>
public interface ICustomerRepository : IRepository<Customer, Guid>
{
    /// <summary>
    /// Gets a customer by customer number.
    /// </summary>
    Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a customer by email.
    /// </summary>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets customers with pagination.
    /// </summary>
    Task<List<Customer>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of customers matching search term.
    /// </summary>
    Task<int> GetCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for customers.
    /// </summary>
    Task<List<Customer>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for Vendor aggregate.
/// </summary>
public interface IVendorRepository : IRepository<Vendor, Guid>
{
    /// <summary>
    /// Gets a vendor by email.
    /// </summary>
    Task<Vendor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets vendors with pagination.
    /// </summary>
    Task<List<Vendor>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of vendors matching search term.
    /// </summary>
    Task<int> GetCountAsync(string? searchTerm = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for vendors.
    /// </summary>
    Task<List<Vendor>> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for Contact entity.
/// </summary>
public interface IContactRepository : IRepository<Contact, Guid>
{
    /// <summary>
    /// Gets contacts by customer ID.
    /// </summary>
    Task<List<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets contacts by vendor ID.
    /// </summary>
    Task<List<Contact>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for Address entity.
/// </summary>
public interface IAddressRepository : IRepository<Address, Guid>
{
    /// <summary>
    /// Gets addresses by customer ID.
    /// </summary>
    Task<List<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets addresses by vendor ID.
    /// </summary>
    Task<List<Address>> GetByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken = default);
}
