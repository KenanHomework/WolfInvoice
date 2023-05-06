namespace WolfInvoice.Models.DocumentModels;

/// <summary>
/// Represents a payment address.
/// </summary>
public class PaymentAddress
{
    /// <summary>
    /// Gets or sets the name associated with the payment address.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address associated with the payment address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email associated with the payment address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number associated with the payment address.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}
