using WolfInvoice.Interfaces.Documents;
using WolfInvoice.Models.DocumentModels;

namespace WolfInvoice.Configurations;

/// <summary>
/// Configuration object for company information that implements the <see cref="IPaymentAddress"/> interface
/// </summary>
public class CompanyInfoConfig : IPaymentAddress
{
    /// <summary>
    /// Gets or sets the name of the company
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the company
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email of the company
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the company
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <inheritdoc/>
    public PaymentAddress GetPaymentAddress() =>
        new()
        {
            Name = Name,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Address = Address,
        };
}
