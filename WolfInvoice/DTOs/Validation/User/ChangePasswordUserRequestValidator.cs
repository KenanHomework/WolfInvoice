using FluentValidation;
using WolfInvoice.DTOs.Requests.User;
using WolfInvoice.DTOs.Validation.Shared;

namespace WolfInvoice.DTOs.Validation.User;

/// <summary>
/// Validator for ChangePasswordUserRequest. Ensures that the CurrentPassword and NewPassword are not empty, are at least 8 characters long, and meet the password validation criteria specified in SharedValidators.
/// </summary>
public class ChangePasswordUserRequestValidator : AbstractValidator<ChangePasswordUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordUserRequestValidator"/> class.
    /// </summary>
    public ChangePasswordUserRequestValidator()
    {
        RuleFor(r => r.CurrentPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Must(SharedValidators.BeValidatePassword);

        RuleFor(r => r.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Must(SharedValidators.BeValidatePassword);
    }
}
