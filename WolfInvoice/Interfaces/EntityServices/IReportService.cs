using WolfInvoice.DTOs.Reports;
using WolfInvoice.Enums;

namespace WolfInvoice.Interfaces.EntityServices;

/// <summary>
/// Provides methods for generating various reports.
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Gets a report of customer activity for a given time period.
    /// </summary>
    /// <param name="period">The time period to generate the report for.</param>
    /// <returns>A customer report.</returns>
    public Task<CustomerReport> GetCustomerReport(TimePeriod period);

    /// <summary>
    /// Gets a report of invoice activity for a given time period.
    /// </summary>
    /// <param name="period">The time period to generate the report for.</param>
    /// <returns>An invoice report.</returns>
    public Task<InvoiceByStatusReport> GetInvoiceByStatusReport(TimePeriod period);

    /// <summary>
    /// Gets a report of invoice activity for a given time period and status.
    /// </summary>
    /// <param name="period">The time period to generate the report for.</param>
    /// <param name="status">The status to filter the report by.</param>
    /// <returns>An invoice report.</returns>
    public Task<InvoiceReport> GetInvoiceReport(TimePeriod period, InvoiceStatus? status = null);
}
