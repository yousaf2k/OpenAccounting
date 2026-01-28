using FluentValidation;
using Customer.Application.Commands;

namespace Customer.Application.Validators;

/// <summary>
/// Validator for CreateCustomerCommand.
/// </summary>
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerNumber)
            .NotEmpty()
            .WithMessage("Customer number is required")
            .MaximumLength(50)
            .WithMessage("Customer number must not exceed 50 characters");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required")
            .MaximumLength(255)
            .WithMessage("Company name must not exceed 255 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone must not exceed 20 characters");

        RuleFor(x => x.TaxId)
            .MaximumLength(50)
            .WithMessage("Tax ID must not exceed 50 characters");

        RuleFor(x => x.Website)
            .MaximumLength(255)
            .WithMessage("Website must not exceed 255 characters");

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Credit limit must be greater than or equal to 0");

        RuleFor(x => x.CustomerType)
            .NotNull()
            .WithMessage("Customer type is required");

        RuleFor(x => x.PaymentTerms)
            .NotNull()
            .WithMessage("Payment terms are required");
    }
}

/// <summary>
/// Validator for UpdateCustomerCommand.
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.CompanyName)
            .MaximumLength(255)
            .WithMessage("Company name must not exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.CompanyName));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.TaxId)
            .MaximumLength(50)
            .WithMessage("Tax ID must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.TaxId));

        RuleFor(x => x.Website)
            .MaximumLength(255)
            .WithMessage("Website must not exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Website));
    }
}

/// <summary>
/// Validator for DeleteCustomerCommand.
/// </summary>
public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");
    }
}

/// <summary>
/// Validator for AddContactToCustomerCommand.
/// </summary>
public class AddContactToCustomerCommandValidator : AbstractValidator<AddContactToCustomerCommand>
{
    public AddContactToCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone must not exceed 20 characters");

        RuleFor(x => x.Mobile)
            .MaximumLength(20)
            .WithMessage("Mobile must not exceed 20 characters");

        RuleFor(x => x.Position)
            .MaximumLength(100)
            .WithMessage("Position must not exceed 100 characters");
    }
}

/// <summary>
/// Validator for RemoveContactFromCustomerCommand.
/// </summary>
public class RemoveContactFromCustomerCommandValidator : AbstractValidator<RemoveContactFromCustomerCommand>
{
    public RemoveContactFromCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.ContactId)
            .NotEmpty()
            .WithMessage("Contact ID is required");
    }
}

/// <summary>
/// Validator for AddAddressToCustomerCommand.
/// </summary>
public class AddAddressToCustomerCommandValidator : AbstractValidator<AddAddressToCustomerCommand>
{
    public AddAddressToCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.Street1)
            .NotEmpty()
            .WithMessage("Street address is required")
            .MaximumLength(255)
            .WithMessage("Street address must not exceed 255 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required")
            .MaximumLength(100)
            .WithMessage("State must not exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required")
            .MaximumLength(20)
            .WithMessage("Postal code must not exceed 20 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.AddressType)
            .NotNull()
            .WithMessage("Address type is required");
    }
}

/// <summary>
/// Validator for RemoveAddressFromCustomerCommand.
/// </summary>
public class RemoveAddressFromCustomerCommandValidator : AbstractValidator<RemoveAddressFromCustomerCommand>
{
    public RemoveAddressFromCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithMessage("Address ID is required");
    }
}

/// <summary>
/// Validator for UpdateCreditLimitCommand.
/// </summary>
public class UpdateCreditLimitCommandValidator : AbstractValidator<UpdateCreditLimitCommand>
{
    public UpdateCreditLimitCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.NewCreditLimit)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Credit limit must be greater than or equal to 0");
    }
}
