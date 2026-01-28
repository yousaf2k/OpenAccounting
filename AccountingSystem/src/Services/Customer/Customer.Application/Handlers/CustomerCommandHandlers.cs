using MediatR;
using AutoMapper;
using Customer.Application.Commands;
using Customer.Application.DTOs;
using Customer.Domain.Entities;
using Customer.Domain.Events;
using Customer.Infrastructure.Repositories;
using BuildingBlocks.EventBus;
using Serilog;

namespace Customer.Application.Handlers;

/// <summary>
/// Handler for creating a customer command.
/// </summary>
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEventBus eventBus)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<CreateCustomerCommandHandler>();
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Creating customer: {CompanyName}", request.CompanyName);

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            CustomerNumber = request.CustomerNumber,
            CompanyName = request.CompanyName,
            TaxId = request.TaxId,
            Email = request.Email,
            Phone = request.Phone,
            Website = request.Website,
            CustomerType = request.CustomerType,
            PaymentTerms = request.PaymentTerms,
            CreditLimit = request.CreditLimit,
            Notes = request.Notes
        };

        // Add domain event before saving
        var domainEvent = new CustomerCreatedDomainEvent(customer.Id, customer.CompanyName, customer.Email);
        customer.AddDomainEvent(domainEvent);

        await _customerRepository.AddAsync(customer, cancellationToken);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer created successfully with ID: {CustomerId}", customer.Id);
        return customer.Id;
    }
}

/// <summary>
/// Handler for updating a customer command.
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<UpdateCustomerCommandHandler>();
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Updating customer: {CustomerId}", request.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        customer.CompanyName = request.CompanyName ?? customer.CompanyName;
        customer.TaxId = request.TaxId ?? customer.TaxId;
        customer.Email = request.Email ?? customer.Email;
        customer.Phone = request.Phone ?? customer.Phone;
        customer.Website = request.Website ?? customer.Website;
        customer.Notes = request.Notes ?? customer.Notes;

        // Raise domain event for update
        var domainEvent = new CustomerUpdatedDomainEvent(customer.Id, customer.CompanyName);
        customer.AddDomainEvent(domainEvent);

        _customerRepository.Update(customer);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer updated successfully: {CustomerId}", request.CustomerId);
        return true;
    }
}

/// <summary>
/// Handler for deleting a customer command.
/// </summary>
public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<DeleteCustomerCommandHandler>();
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Deleting customer: {CustomerId}", request.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        // Soft delete
        customer.IsDeleted = true;
        customer.DeletedAt = DateTime.UtcNow;

        // Raise domain event for deletion
        var domainEvent = new CustomerDeletedDomainEvent(customer.Id, customer.CompanyName);
        customer.AddDomainEvent(domainEvent);

        _customerRepository.Update(customer);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Customer deleted successfully: {CustomerId}", request.CustomerId);
        return true;
    }
}

/// <summary>
/// Handler for adding a contact to a customer.
/// </summary>
public class AddContactToCustomerCommandHandler : IRequestHandler<AddContactToCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AddContactToCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<AddContactToCustomerCommandHandler>();
    }

    public async Task<bool> Handle(AddContactToCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Adding contact to customer: {CustomerId}", request.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Mobile = request.Mobile,
            Position = request.Position,
            IsPrimary = request.IsPrimary,
            CustomerId = request.CustomerId
        };

        customer.AddContact(contact);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Contact added to customer successfully: {CustomerId}", request.CustomerId);
        return true;
    }
}

/// <summary>
/// Handler for removing a contact from a customer.
/// </summary>
public class RemoveContactFromCustomerCommandHandler : IRequestHandler<RemoveContactFromCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public RemoveContactFromCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<RemoveContactFromCustomerCommandHandler>();
    }

    public async Task<bool> Handle(RemoveContactFromCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Removing contact from customer: {CustomerId}, {ContactId}", request.CustomerId, request.ContactId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        var contact = await _contactRepository.GetByIdAsync(request.ContactId, cancellationToken);
        if (contact == null)
        {
            _logger.Warning("Contact not found: {ContactId}", request.ContactId);
            return false;
        }

        customer.RemoveContact(contact);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Contact removed from customer successfully");
        return true;
    }
}

/// <summary>
/// Handler for adding an address to a customer.
/// </summary>
public class AddAddressToCustomerCommandHandler : IRequestHandler<AddAddressToCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AddAddressToCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<AddAddressToCustomerCommandHandler>();
    }

    public async Task<bool> Handle(AddAddressToCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Adding address to customer: {CustomerId}", request.CustomerId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        var address = new Address
        {
            Id = Guid.NewGuid(),
            AddressType = request.AddressType,
            Street1 = request.Street1,
            Street2 = request.Street2,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            IsPrimary = request.IsPrimary,
            CustomerId = request.CustomerId
        };

        customer.AddAddress(address);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Address added to customer successfully: {CustomerId}", request.CustomerId);
        return true;
    }
}

/// <summary>
/// Handler for removing an address from a customer.
/// </summary>
public class RemoveAddressFromCustomerCommandHandler : IRequestHandler<RemoveAddressFromCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public RemoveAddressFromCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<RemoveAddressFromCustomerCommandHandler>();
    }

    public async Task<bool> Handle(RemoveAddressFromCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Removing address from customer: {CustomerId}, {AddressId}", request.CustomerId, request.AddressId);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
        if (address == null)
        {
            _logger.Warning("Address not found: {AddressId}", request.AddressId);
            return false;
        }

        customer.RemoveAddress(address);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Address removed from customer successfully");
        return true;
    }
}

/// <summary>
/// Handler for updating customer credit limit.
/// </summary>
public class UpdateCreditLimitCommandHandler : IRequestHandler<UpdateCreditLimitCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public UpdateCreditLimitCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<UpdateCreditLimitCommandHandler>();
    }

    public async Task<bool> Handle(UpdateCreditLimitCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Updating credit limit for customer: {CustomerId}, NewLimit: {CreditLimit}", request.CustomerId, request.NewCreditLimit);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            _logger.Warning("Customer not found: {CustomerId}", request.CustomerId);
            return false;
        }

        customer.UpdateCreditLimit(request.NewCreditLimit);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Credit limit updated successfully for customer: {CustomerId}", request.CustomerId);
        return true;
    }
}
