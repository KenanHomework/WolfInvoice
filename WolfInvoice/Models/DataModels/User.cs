using WolfInvoice.Enums;

namespace WolfInvoice.Models.DataModels;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public string? Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date and time of the user record.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date and time of the user record.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Represents the life status of customer
    /// </summary>
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;

    /* Navigation Properties */

    /// <summary>
    /// User created invoices
    /// </summary>
    public HashSet<Invoice> Invoices { get; set; } = new();

    /// <summary>
    /// User created customers
    /// </summary>
    public HashSet<Customer> Customers { get; set; } = new();
}
