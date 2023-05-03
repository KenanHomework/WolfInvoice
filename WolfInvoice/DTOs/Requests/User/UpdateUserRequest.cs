namespace WolfInvoice.DTOs.Requests.User;

/// <summary>
/// User update request DTO
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the user.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }
}
