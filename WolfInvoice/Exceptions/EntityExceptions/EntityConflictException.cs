namespace WolfInvoice.Exceptions.EntityExceptions;

/// <summary>
/// This class represents an exception that is thrown when there is a conflict with an entity.
/// </summary>
public class EntityConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityConflictException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    public EntityConflictException(string? message = null)
        : base(message) { }
}
