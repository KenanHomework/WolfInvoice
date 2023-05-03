namespace WolfInvoice.DTOs.Reports;

/// <summary>
/// Represents the statistics of invoices grouped by their status.
/// </summary>
public class InvoiceByStatusReport
{
    /// <summary>
    /// Gets or sets the statistics of invoices that have been rejected.
    /// </summary>
    public InvoiceReport? StatisticsOfRejectedStatus { get; set; }

    /// <summary>
    /// Gets or sets the statistics of invoices that have been cancelled.
    /// </summary>
    public InvoiceReport? StatisticsOfCancelledStatus { get; set; }

    /// <summary>
    /// Gets or sets the statistics of invoices that have been paid.
    /// </summary>
    public InvoiceReport? StatisticsOfPaidStatus { get; set; }

    /// <summary>
    /// Gets or sets the statistics of invoices that have been received.
    /// </summary>
    public InvoiceReport? StatisticsOfReceivedStatus { get; set; }

    /// <summary>
    /// Gets or sets the statistics of invoices that have been sent.
    /// </summary>
    public InvoiceReport? StatisticsOfSentStatus { get; set; }
}
