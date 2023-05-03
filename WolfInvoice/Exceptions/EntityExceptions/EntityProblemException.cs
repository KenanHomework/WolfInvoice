namespace WolfInvoice.Exceptions.EntityExceptions;

/// <summary>
/// This class represents an exception that is thrown when there is problem accured of entity.
/// </summary>
public class EntityProblemException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityProblemException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    public EntityProblemException(string? message = null)
        : base(message) { }
}
