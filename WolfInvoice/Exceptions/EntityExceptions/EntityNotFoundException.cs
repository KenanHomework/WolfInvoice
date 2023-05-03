namespace WolfInvoice.Exceptions.EntityExceptions;

/// <summary>
/// This class represents an exception that is thrown when there is a not found an entity.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the exception.</param>
    public EntityNotFoundException(string? message = null)
        : base(message) { }
}
