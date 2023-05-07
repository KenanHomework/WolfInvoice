using WolfInvoice.Models.DataModels;

namespace WolfInvoice.DTOs.Entities;

/// <summary>
/// Represents a customer data transfer object.
/// </summary>
public class CustomerDto
{
    /// <summary>
    /// Initialize dto
    /// </summary>
    public CustomerDto() { }

    /// <summary>
    /// Initialize dto with Customer
    /// </summary>
    /// <param name="customer"></param>
    public CustomerDto(Customer customer)
    {
        Id = customer.Id;
        Name = customer.Name;
        Address = customer.Address;
        CreatedAt = customer.CreatedAt;
        Email = customer.Email;
        PhoneNumber = customer.PhoneNumber;
        CreatedAt = customer.CreatedAt;
        CreditCard = customer.CreditCard;
    }

    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer credit card information.
    /// </summary>
    public string CreditCard { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer creation date and time.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
