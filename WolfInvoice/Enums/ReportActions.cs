using System.Text.Json.Serialization;

namespace WolfInvoice.Enums;

/// <summary>
/// Actions that can be performed for generating reports.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportActions
{
    /// <summary>
    /// Generate a report on customers, including the number of invoices and total amount on accounts during the specified period.
    /// </summary>
    CustomersReport,

    /// <summary>
    /// Generate a report on completed works, including the number of invoices and total amount on accounts during the specified period.
    /// </summary>
    CompletedWorksReport,

    /// <summary>
    /// Generate a report on invoices, including a count for each status during the specified period.
    /// </summary>
    InvoicesReport
}
