using System.Text.Json.Serialization;

namespace WolfInvoice.DTOs.Reports;

/// <summary>
/// Represents a time period with a start and end date/time.
/// </summary>
public class TimePeriod
{
    /// <summary>
    /// Gets or sets the start date/time of the period.
    /// </summary>
    public DateTimeOffset? Start { get; set; }

    /// <summary>
    /// Gets or sets the end date/time of the period.
    /// </summary>
    public DateTimeOffset? End { get; set; }
}
