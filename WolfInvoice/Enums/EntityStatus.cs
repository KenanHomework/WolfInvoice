using System.Text.Json.Serialization;

namespace WolfInvoice.Enums;

/// <summary>
/// Represents the life status of an entity.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
public enum EntityStatus
{
    /// <summary>
    /// The entity is active.
    /// </summary>
    Active,

    /// <summary>
    /// The entity has been soft-deleted.
    /// </summary>
    Deleted,

    /// <summary>
    /// The entity has been archived.
    /// </summary>
    Archived
}
