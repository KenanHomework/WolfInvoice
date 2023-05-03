namespace WolfInvoice.Services;

/// <summary>
/// Service for generating unique identifiers.
/// </summary>
public static class IdGeneratorService
{
    /// <summary>
    /// Generates a new unique identifier.
    /// </summary>
    /// <returns>A string representing the new unique identifier.</returns>
    public static string GetUniqueId() => Guid.NewGuid().ToString("N").ToLower();
}
