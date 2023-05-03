using FluentValidation;
using WolfInvoice.DTOs.Requests.Invoice;

namespace WolfInvoice.DTOs.Validation.Invoice;

/// <summary>
/// Validator for <see cref="CreateInvoiceRequest"/>.
/// </summary>
public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateInvoiceRequestValidator"/> class.
    /// </summary>
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.Discount)
            .NotNull()
            .When(x => x.Discount.HasValue)
            .WithMessage("Discount should not be null.");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Comment))
            .WithMessage("Comment should be less than or equal to 500 characters.");

        RuleFor(x => x.Rows)
            .Must(x => x != null && x.Any())
            .When(x => x.Rows != null)
            .WithMessage("At least one row is required.")
            .ForEach(x => x.SetValidator(new CreateInvoiceRowRequestValidator()));
    }
}
