namespace WolfInvoice.Enums;

/// <summary>
/// Enumeration of user actions that can be logged.
/// </summary>
public enum UserActions
{
    /// <summary>
    /// User account was created.
    /// </summary>
    Created,

    /// <summary>
    /// User account was added.
    /// </summary>
    Added,

    /// <summary>
    /// User logged in.
    /// </summary>
    Logged,

    /// <summary>
    /// User registered.
    /// </summary>
    Registered,

    /// <summary>
    /// User changed their password.
    /// </summary>
    PasswordChanged,

    /// <summary>
    /// User information was edited.
    /// </summary>
    Edited,

    /// <summary>
    /// User account was deleted.
    /// </summary>
    Deleted
}
