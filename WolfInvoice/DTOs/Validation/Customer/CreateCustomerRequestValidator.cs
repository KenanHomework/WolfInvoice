using FluentValidation;
using WolfInvoice.DTOs.Requests.Customer;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.Customer;

/// <summary>
/// Validator for creating a new customer.
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerRequestValidator"/> class.
    /// </summary>
    public CreateCustomerRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().Must(SharedValidators.BeValidateEmail);

        RuleFor(r => r.Name).NotEmpty().MinimumLength(3).Must(SharedValidators.BeValidateUsername);

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(20)
            .Must(SharedValidators.BeValidatePhoneNumber);

        RuleFor(r => r.Address).NotEmpty();

        RuleFor(r => r.CreditCard).NotEmpty().Must(SharedValidators.BeValidateCardNumber);
    }
}
