namespace WolfInvoice.DTOs.Reports;

/// <summary>
/// Contains statistical information on customer activity.
/// </summary>
public class CustomerReport
{
    /// <summary>
    /// The average number of invoices per customer.
    /// </summary>
    public decimal AverageInvoicePerCustomer { get; set; }

    /// <summary>
    /// The average price of invoices per customer.
    /// </summary>
    public decimal AverageInvoicePricePerCustomer { get; set; }

    /// <summary>
    /// The average cost of invoices per customer.
    /// </summary>
    public decimal AverageCostPerCustomer { get; set; }
}
