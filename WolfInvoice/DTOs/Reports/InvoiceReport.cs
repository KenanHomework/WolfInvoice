namespace WolfInvoice.DTOs.Reports;

/// <summary>
/// Represents a report about invoices.
/// </summary>
public class InvoiceReport
{
    /// <summary>
    /// Gets or sets the total number of invoices in the report.
    /// </summary>
    public int TotalInvoiceCount { get; set; }

    /// <summary>
    /// Gets or sets the total cost of the invoices in the report.
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// Gets or sets the average price of the invoices in the report.
    /// </summary>
    public decimal AveragePrice { get; set; }
}
