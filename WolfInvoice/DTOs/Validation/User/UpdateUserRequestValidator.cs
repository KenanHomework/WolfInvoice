using FluentValidation;
using WolfInvoice.DTOs.Requests.User;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.User;

/// <summary>
/// Validator for updating a  user.
/// </summary>
public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserRequestValidator"/> class.
    /// </summary>
    public UpdateUserRequestValidator()
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

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .MinimumLength(10)
            .MaximumLength(20)
            .Must(SharedValidators.BeValidatePhoneNumber)
            .When(r => r.PhoneNumber is not null);
    }
}
