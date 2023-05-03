namespace WolfInvoice.DTOs.Requests.User;

/// <summary>
/// User create request DTO
/// </summary>
public class CreateUserRequest
{
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
}
