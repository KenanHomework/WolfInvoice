using FluentValidation;
using WolfInvoice.DTOs.Requests.Auth;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.Auth;

/// <summary>
/// Validator for login a  user.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginRequestValidator"/> class.
    /// </summary>
    public LoginRequestValidator()
    {
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
