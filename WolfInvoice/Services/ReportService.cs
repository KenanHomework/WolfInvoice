using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Reports;
using WolfInvoice.Enums;
using WolfInvoice.Interfaces.EntityServices;

namespace WolfInvoice.Services;

/// <summary>
/// Provides methods for generating various reports.
/// </summary>
public class ReportService : IReportService
{
    private readonly WolfInvoiceContext _context;

    /// <summary>
    /// Initialize object
    /// </summary>
    /// <param name="context"></param>
    public ReportService(WolfInvoiceContext context) => _context = context;

    /// <inheritdoc/>
    public async Task<CustomerReport> GetCustomerReport(TimePeriod period)
    {
        CheckPeriod(period);

        CustomerReport customerReport = new();

        var customers = await _context.Customers
            .Include(c => c.Invoices)
            .Where(c => c.CreatedAt >= period.Start && c.CreatedAt <= period.End)
            .ToListAsync();

        customerReport.AverageCostPerCustomer =
            customers.Average(c => c.Invoices.Sum(i => i.TotalSum)) / customers.Count;
        customerReport.AverageInvoicePerCustomer =
            (decimal)customers.Average(c => c.Invoices.Count) / customers.Count;
        customerReport.AverageInvoicePricePerCustomer = customers.Average(
            c => c.Invoices.Sum(i => i.TotalSum) / c.Invoices.Count
        );

        return customerReport;
    }

    /// <inheritdoc/>
    public async Task<InvoiceByStatusReport> GetInvoiceByStatusReport(TimePeriod period) =>
        new()
        {
            StatisticsOfCancelledStatus = await GetInvoiceReport(period, InvoiceStatus.Cancelled),
            StatisticsOfPaidStatus = await GetInvoiceReport(period, InvoiceStatus.Paid),
            StatisticsOfReceivedStatus = await GetInvoiceReport(period, InvoiceStatus.Received),
            StatisticsOfRejectedStatus = await GetInvoiceReport(period, InvoiceStatus.Rejected),
            StatisticsOfSentStatus = await GetInvoiceReport(period, InvoiceStatus.Sent)
        };

    /// <inheritdoc/>
    public async Task<InvoiceReport> GetInvoiceReport(
        TimePeriod period,
        InvoiceStatus? status = null
    )
    {
        CheckPeriod(period);

        InvoiceReport report = new();

        var query = _context.Invoices.Where(
            i => i.CreatedAt >= period.Start && i.CreatedAt <= period.End
        );

        if (status.HasValue)
            query = query.Where(i => i.Status == status);

        decimal invoicesCost = await query.SumAsync(i => i.TotalSum);
        int invoiceCount = await query.CountAsync();

        report.TotalCost = invoicesCost;
        report.TotalInvoiceCount = invoiceCount;
        report.AveragePrice = invoicesCost / invoiceCount;

        return report;
    }

    private static void CheckPeriod(TimePeriod period)
    {
        if (period is null)
            throw new Exception("The period can't be null");

        if (DateTimeOffset.Now < period.Start)
            throw new Exception("The start time of the period is not correct");

        if (DateTimeOffset.Now < period.End)
            throw new Exception("The end time of the period is not correct");

        if (period.Start > period.End)
            throw new Exception("The start time of the period can't be greater than the end time");
    }
}
