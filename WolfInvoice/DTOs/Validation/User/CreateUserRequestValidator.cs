using FluentValidation;
using WolfInvoice.DTOs.Requests.User;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.User;

/// <summary>
/// Validator for creating a new user.
/// </summary>
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserRequestValidator"/> class.
    /// </summary>
    public CreateUserRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().Must(SharedValidators.BeValidateEmail);

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Must(SharedValidators.BeValidatePassword);

        RuleFor(r => r.Name).NotEmpty().MinimumLength(4).Must(SharedValidators.BeValidateUsername);

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(20)
            .Must(SharedValidators.BeValidatePhoneNumber)
            .When(r => r.PhoneNumber is not null);
    }
}
