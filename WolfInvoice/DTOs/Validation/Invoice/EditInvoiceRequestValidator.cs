using FluentValidation;
using WolfInvoice.DTOs.Requests.Invoice;

namespace WolfInvoice.DTOs.Validation.Invoice;

/// <summary>
/// Validator for editing an invoice request.
/// </summary>
public class EditInvoiceRequestValidator : AbstractValidator<EditInvoiceRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditInvoiceRequestValidator"/> class.
    /// </summary>
    public EditInvoiceRequestValidator()
    {
        RuleFor(x => x.Discount)
            .Must(x => x >= 0 && x <= 100)
            .WithMessage("Discount must be between 0 and 100.")
            .When(x => x.Discount is not null);
        ;

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Comment must be at most 500 characters.")
            .When(x => x.Comment is not null);
    }
}
