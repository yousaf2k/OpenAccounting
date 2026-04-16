using MediatR;
using AutoMapper;
using Customer.Application.Queries;
using Customer.Application.DTOs;
using Customer.Infrastructure.Repositories;
using Serilog;

namespace Customer.Application.Handlers;

/// <summary>
/// Handler for getting a customer by ID.
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCustomerByIdQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetCustomerByIdQueryHandler>();
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting customer by ID: {CustomerId}", request.CustomerId);
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }
}

/// <summary>
/// Handler for getting all customers.
/// </summary>
public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetAllCustomersQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetAllCustomersQueryHandler>();
    }

    public async Task<PagedResult<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting all customers with pagination - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

        var customers = await _customerRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        var totalCount = await _customerRepository.GetCountAsync(request.SearchTerm, cancellationToken);

        var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

        return new PagedResult<CustomerDto>
        {
            Items = customerDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

/// <summary>
/// Handler for getting a customer by customer number.
/// </summary>
public class GetCustomerByNumberQueryHandler : IRequestHandler<GetCustomerByNumberQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCustomerByNumberQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetCustomerByNumberQueryHandler>();
    }

    public async Task<CustomerDto?> Handle(GetCustomerByNumberQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting customer by number: {CustomerNumber}", request.CustomerNumber);
        var customer = await _customerRepository.GetByCustomerNumberAsync(request.CustomerNumber, cancellationToken);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }
}

/// <summary>
/// Handler for getting a customer by email.
/// </summary>
public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCustomerByEmailQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetCustomerByEmailQueryHandler>();
    }

    public async Task<CustomerDto?> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting customer by email: {Email}", request.Email);
        var customer = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }
}

/// <summary>
/// Handler for searching customers.
/// </summary>
public class SearchCustomersQueryHandler : IRequestHandler<SearchCustomersQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SearchCustomersQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<SearchCustomersQueryHandler>();
    }

    public async Task<List<CustomerDto>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Searching customers with term: {SearchTerm}", request.SearchTerm);
        var customers = await _customerRepository.SearchAsync(request.SearchTerm, request.PageNumber, request.PageSize, cancellationToken);
        return _mapper.Map<List<CustomerDto>>(customers);
    }
}

/// <summary>
/// Handler for getting a vendor by ID.
/// </summary>
public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto?>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetVendorByIdQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetVendorByIdQueryHandler>();
    }

    public async Task<VendorDto?> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting vendor by ID: {VendorId}", request.VendorId);
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        return vendor == null ? null : _mapper.Map<VendorDto>(vendor);
    }
}

/// <summary>
/// Handler for getting all vendors.
/// </summary>
public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, PagedResult<VendorDto>>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetAllVendorsQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetAllVendorsQueryHandler>();
    }

    public async Task<PagedResult<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting all vendors with pagination - Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

        var vendors = await _vendorRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);

        var totalCount = await _vendorRepository.GetCountAsync(request.SearchTerm, cancellationToken);

        var vendorDtos = _mapper.Map<List<VendorDto>>(vendors);

        return new PagedResult<VendorDto>
        {
            Items = vendorDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

/// <summary>
/// Handler for getting a vendor by email.
/// </summary>
public class GetVendorByEmailQueryHandler : IRequestHandler<GetVendorByEmailQuery, VendorDto?>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetVendorByEmailQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetVendorByEmailQueryHandler>();
    }

    public async Task<VendorDto?> Handle(GetVendorByEmailQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting vendor by email: {Email}", request.Email);
        var vendor = await _vendorRepository.GetByEmailAsync(request.Email, cancellationToken);
        return vendor == null ? null : _mapper.Map<VendorDto>(vendor);
    }
}

/// <summary>
/// Handler for searching vendors.
/// </summary>
public class SearchVendorsQueryHandler : IRequestHandler<SearchVendorsQuery, List<VendorDto>>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SearchVendorsQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<SearchVendorsQueryHandler>();
    }

    public async Task<List<VendorDto>> Handle(SearchVendorsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Searching vendors with term: {SearchTerm}", request.SearchTerm);
        var vendors = await _vendorRepository.SearchAsync(request.SearchTerm, request.PageNumber, request.PageSize, cancellationToken);
        return _mapper.Map<List<VendorDto>>(vendors);
    }
}

/// <summary>
/// Handler for getting vendor contacts.
/// </summary>
public class GetVendorContactsQueryHandler : IRequestHandler<GetVendorContactsQuery, List<ContactDto>>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetVendorContactsQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetVendorContactsQueryHandler>();
    }

    public async Task<List<ContactDto>> Handle(GetVendorContactsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting contacts for vendor: {VendorId}", request.VendorId);
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            return new List<ContactDto>();
        }
        return _mapper.Map<List<ContactDto>>(vendor.Contacts);
    }
}

/// <summary>
/// Handler for getting vendor addresses.
/// </summary>
public class GetVendorAddressesQueryHandler : IRequestHandler<GetVendorAddressesQuery, List<AddressDto>>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetVendorAddressesQueryHandler(
        IVendorRepository vendorRepository,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetVendorAddressesQueryHandler>();
    }

    public async Task<List<AddressDto>> Handle(GetVendorAddressesQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting addresses for vendor: {VendorId}", request.VendorId);
        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            return new List<AddressDto>();
        }
        return _mapper.Map<List<AddressDto>>(vendor.Addresses);
    }
}

/// <summary>
/// Handler for getting a contact by ID.
/// </summary>
public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetContactByIdQueryHandler(
        IContactRepository contactRepository,
        IMapper mapper)
    {
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetContactByIdQueryHandler>();
    }

    public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting contact by ID: {ContactId}", request.ContactId);
        var contact = await _contactRepository.GetByIdAsync(request.ContactId, cancellationToken);
        return contact == null ? null : _mapper.Map<ContactDto>(contact);
    }
}

/// <summary>
/// Handler for getting customer contacts.
/// </summary>
public class GetCustomerContactsQueryHandler : IRequestHandler<GetCustomerContactsQuery, List<ContactDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCustomerContactsQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetCustomerContactsQueryHandler>();
    }

    public async Task<List<ContactDto>> Handle(GetCustomerContactsQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting contacts for customer: {CustomerId}", request.CustomerId);
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new List<ContactDto>();
        }
        return _mapper.Map<List<ContactDto>>(customer.Contacts);
    }
}

/// <summary>
/// Handler for getting an address by ID.
/// </summary>
public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, AddressDto?>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetAddressByIdQueryHandler(
        IAddressRepository addressRepository,
        IMapper mapper)
    {
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetAddressByIdQueryHandler>();
    }

    public async Task<AddressDto?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting address by ID: {AddressId}", request.AddressId);
        var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
        return address == null ? null : _mapper.Map<AddressDto>(address);
    }
}

/// <summary>
/// Handler for getting customer addresses.
/// </summary>
public class GetCustomerAddressesQueryHandler : IRequestHandler<GetCustomerAddressesQuery, List<AddressDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetCustomerAddressesQueryHandler(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<GetCustomerAddressesQueryHandler>();
    }

    public async Task<List<AddressDto>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Getting addresses for customer: {CustomerId}", request.CustomerId);
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new List<AddressDto>();
        }
        return _mapper.Map<List<AddressDto>>(customer.Addresses);
    }
}
