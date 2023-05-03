using FluentValidation;
using FluentValidation.Validators;
using WolfInvoice.DTOs.Requests.Invoice;

namespace WolfInvoice.DTOs.Validation.Invoice;

/// <summary>
/// Validator for CreateInvoiceRowRequest model. Validates the Service, Quantity, Amount and Sum fields.
/// </summary>
public class CreateInvoiceRowRequestValidator : AbstractValidator<CreateInvoiceRowRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateInvoiceRowRequestValidator"/> class.
    /// </summary>
    public CreateInvoiceRowRequestValidator()
    {
        RuleFor(x => x.Service).NotEmpty().WithMessage("Service cannot be empty.");

        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}
