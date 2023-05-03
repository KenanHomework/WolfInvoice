namespace WolfInvoice.DTOs.Requests.Auth;

/// <summary>
/// A class that represents a request for registering a new user.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// The name of the user to be registered.
    /// Email must necessarily be unique.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email of the user to be registered.
    /// Email must necessarily be unique.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password of the user to be registered.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
