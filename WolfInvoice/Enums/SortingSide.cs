using System.Text.Json.Serialization;

namespace WolfInvoice.Enums;

/// <summary>
/// Specifies the sorting order for a query.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortingSide
{
    /// <summary>
    /// Sorts the results in ascending order.
    /// </summary>
    Ascending,

    /// <summary>
    /// Sorts the results in descending order.
    /// </summary>
    Descending
}
