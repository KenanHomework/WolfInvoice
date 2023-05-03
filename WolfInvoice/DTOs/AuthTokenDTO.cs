namespace WolfInvoice.DTOs;

/// <summary>
/// Data transfer object for the authentication token.
/// </summary>
public class AuthTokenDto
{
    /// <summary>
    /// Gets or sets the access token string.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}
