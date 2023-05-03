namespace WolfInvoice.DTOs.Requests.Customer;

/// <summary>
/// Customer update request DTO
/// </summary>
public class EditCustomerRequest
{
    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the customer.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the credit card information of the customer.
    /// </summary>
    public string? CreditCard { get; set; }

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    public string? PhoneNumber { get; set; }
}
