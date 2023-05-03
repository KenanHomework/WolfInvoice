namespace WolfInvoice.Exceptions.EntityExceptions;

/// <summary>
/// This class represents an exception that is thrown when there is password invalid of entity.
/// </summary>
public class EntityPasswordInvalidException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityPasswordInvalidException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    public EntityPasswordInvalidException(string? message = null)
        : base(message) { }
}
