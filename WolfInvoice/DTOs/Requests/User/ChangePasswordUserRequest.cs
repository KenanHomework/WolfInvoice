namespace WolfInvoice.DTOs.Requests.User;

/// <summary>
/// User change password request DTO
/// </summary>
public class ChangePasswordUserRequest
{
    /// <summary>
    /// Gets or sets the current password of the user.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password of the user.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;
}
