namespace WolfInvoice.DTOs.Requests.Auth;

/// <summary>
/// A data transfer object used for user login requests.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// Email must necessarily be unique.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user's account.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
