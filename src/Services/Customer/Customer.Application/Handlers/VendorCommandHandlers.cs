using MediatR;
using AutoMapper;
using Customer.Application.Commands;
using Customer.Domain.Entities;
using Customer.Domain.Events;
using Customer.Infrastructure.Repositories;
using BuildingBlocks.EventBus;
using Serilog;

namespace Customer.Application.Handlers;

/// <summary>
/// Handler for creating a vendor command.
/// </summary>
public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, Guid>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public CreateVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEventBus eventBus)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = Log.ForContext<CreateVendorCommandHandler>();
    }

    public async Task<Guid> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Creating vendor: {CompanyName}", request.CompanyName);

        var vendor = new Vendor
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            TaxId = request.TaxId,
            Email = request.Email,
            Phone = request.Phone,
            Website = request.Website,
            VendorType = request.VendorType,
            PaymentTerms = request.PaymentTerms,
            Notes = request.Notes
        };

        // Add domain event before saving
        var domainEvent = new VendorCreatedDomainEvent(vendor.Id, vendor.CompanyName, vendor.Email);
        vendor.AddDomainEvent(domainEvent);

        await _vendorRepository.AddAsync(vendor, cancellationToken);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Vendor created successfully with ID: {VendorId}", vendor.Id);
        return vendor.Id;
    }
}

/// <summary>
/// Handler for updating a vendor command.
/// </summary>
public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public UpdateVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<UpdateVendorCommandHandler>();
    }

    public async Task<bool> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Updating vendor: {VendorId}", request.VendorId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
            return false;
        }

        vendor.CompanyName = request.CompanyName ?? vendor.CompanyName;
        vendor.TaxId = request.TaxId ?? vendor.TaxId;
        vendor.Email = request.Email ?? vendor.Email;
        vendor.Phone = request.Phone ?? vendor.Phone;
        vendor.Website = request.Website ?? vendor.Website;
        vendor.Notes = request.Notes ?? vendor.Notes;

        // Raise domain event for update
        var domainEvent = new VendorUpdatedDomainEvent(vendor.Id, vendor.CompanyName);
        vendor.AddDomainEvent(domainEvent);

        _vendorRepository.Update(vendor);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Vendor updated successfully: {VendorId}", request.VendorId);
        return true;
    }
}

/// <summary>
/// Handler for deleting a vendor command.
/// </summary>
public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public DeleteVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<DeleteVendorCommandHandler>();
    }

    public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Deleting vendor: {VendorId}", request.VendorId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
            return false;
        }

        vendor.IsDeleted = true;
        vendor.DeletedAt = DateTime.UtcNow;

        // Raise domain event for deletion
        var domainEvent = new VendorDeletedDomainEvent(vendor.Id, vendor.CompanyName);
        vendor.AddDomainEvent(domainEvent);

        _vendorRepository.Update(vendor);
        // SaveChangesAsync in DbContext will dispatch the domain event
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Vendor deleted successfully: {VendorId}", request.VendorId);
        return true;
    }
}

/// <summary>
/// Handler for adding a contact to a vendor.
/// </summary>
public class AddContactToVendorCommandHandler : IRequestHandler<AddContactToVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AddContactToVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<AddContactToVendorCommandHandler>();
    }

    public async Task<bool> Handle(AddContactToVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Adding contact to vendor: {VendorId}", request.VendorId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
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
            VendorId = request.VendorId
        };

        vendor.AddContact(contact);
        _vendorRepository.Update(vendor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Contact added to vendor successfully: {VendorId}", request.VendorId);
        return true;
    }
}

/// <summary>
/// Handler for removing a contact from a vendor.
/// </summary>
public class RemoveContactFromVendorCommandHandler : IRequestHandler<RemoveContactFromVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public RemoveContactFromVendorCommandHandler(
        IVendorRepository vendorRepository,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<RemoveContactFromVendorCommandHandler>();
    }

    public async Task<bool> Handle(RemoveContactFromVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Removing contact from vendor: {VendorId}, {ContactId}", request.VendorId, request.ContactId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
            return false;
        }

        var contact = await _contactRepository.GetByIdAsync(request.ContactId, cancellationToken);
        if (contact == null)
        {
            _logger.Warning("Contact not found: {ContactId}", request.ContactId);
            return false;
        }

        vendor.RemoveContact(contact);
        _vendorRepository.Update(vendor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Contact removed from vendor successfully");
        return true;
    }
}

/// <summary>
/// Handler for adding an address to a vendor.
/// </summary>
public class AddAddressToVendorCommandHandler : IRequestHandler<AddAddressToVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AddAddressToVendorCommandHandler(
        IVendorRepository vendorRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = Log.ForContext<AddAddressToVendorCommandHandler>();
    }

    public async Task<bool> Handle(AddAddressToVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Adding address to vendor: {VendorId}", request.VendorId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
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
            VendorId = request.VendorId
        };

        vendor.AddAddress(address);
        _vendorRepository.Update(vendor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Address added to vendor successfully: {VendorId}", request.VendorId);
        return true;
    }
}

/// <summary>
/// Handler for removing an address from a vendor.
/// </summary>
public class RemoveAddressFromVendorCommandHandler : IRequestHandler<RemoveAddressFromVendorCommand, bool>
{
    private readonly IVendorRepository _vendorRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public RemoveAddressFromVendorCommandHandler(
        IVendorRepository vendorRepository,
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork)
    {
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = Log.ForContext<RemoveAddressFromVendorCommandHandler>();
    }

    public async Task<bool> Handle(RemoveAddressFromVendorCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("Removing address from vendor: {VendorId}, {AddressId}", request.VendorId, request.AddressId);

        var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor == null)
        {
            _logger.Warning("Vendor not found: {VendorId}", request.VendorId);
            return false;
        }

        var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
        if (address == null)
        {
            _logger.Warning("Address not found: {AddressId}", request.AddressId);
            return false;
        }

        vendor.RemoveAddress(address);
        _vendorRepository.Update(vendor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Address removed from vendor successfully");
        return true;
    }
}
