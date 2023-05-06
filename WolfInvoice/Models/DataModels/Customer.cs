using WolfInvoice.Enums;
using WolfInvoice.Interfaces.Documents;
using WolfInvoice.Models.DocumentModels;

namespace WolfInvoice.Models.DataModels;

/// <summary>
/// Represents a customer in the system.
/// </summary>
public class Customer : IPaymentAddress
{
    /// <summary>
    /// Gets or sets the unique identifier of the customer.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the customer.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the credit card information of the customer.
    /// </summary>
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the customer was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the customer was last updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the customer was deleted, or null if the customer has not been deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Represents the life status of customer
    /// </summary>
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;

    /* Navigation Properties */

    /// <summary>
    /// Invoices to which the customer is bound
    /// </summary>
    public HashSet<Invoice> Invoices { get; set; } = new();

    /// <summary>
    /// User to who customer is bound
    /// </summary>
    public User User { get; set; } = default!;

    ///  <inheritdoc/>
    public PaymentAddress GetPaymentAddress() =>
        new()
        {
            Name = Name,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Address = Address,
        };
}
