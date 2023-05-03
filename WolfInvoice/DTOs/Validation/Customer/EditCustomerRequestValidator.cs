using FluentValidation;
using WolfInvoice.DTOs.Requests.Customer;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.Customer;

/// <summary>
/// Validator for creating a new customer.
/// </summary>
public class EditCustomerRequestValidator : AbstractValidator<EditCustomerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditCustomerRequestValidator"/> class.
    /// </summary>
    public EditCustomerRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .MinimumLength(5)
            .Must(SharedValidators.BeValidateEmail)
            .When(r => r.Email is not null);

        RuleFor(r => r.Name)
            .NotEmpty()
            .MinimumLength(4)
            .Must(SharedValidators.BeValidateUsername)
            .When(r => r.Name is not null);

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(20)
            .Must(SharedValidators.BeValidatePhoneNumber)
            .When(r => r.PhoneNumber is not null);

        RuleFor(r => r.CreditCard)
            .NotEmpty()
            .MinimumLength(10)
            .Must(SharedValidators.BeValidateCardNumber)
            .When(r => r.CreditCard is not null);
    }
}
