using FluentValidation;
using WolfInvoice.DTOs.Requests.Auth;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.Auth;

/// <summary>
/// Validator for register a  user.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterRequestValidator"/> class.
    /// </summary>
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MinimumLength(4)
            .Must(SharedValidators.BeValidateUsername)
            .When(r => r.Name is not null);

        RuleFor(r => r.Email)
            .NotEmpty()
            .MinimumLength(5)
            .Must(SharedValidators.BeValidateEmail)
            .When(r => r.Email is not null);

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Must(SharedValidators.BeValidatePassword);
    }
}
