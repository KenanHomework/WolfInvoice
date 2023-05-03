namespace WolfInvoice.Providers.RequestUser;

/// <summary>
/// Represents information about the authenticated user making a request.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfo"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="email">The email of the user</param>
    public UserInfo(string id, string email)
    {
        Id = id;
        Email = email;
    }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the email of the user.
    /// </summary>
    public string Email { get; }
}
