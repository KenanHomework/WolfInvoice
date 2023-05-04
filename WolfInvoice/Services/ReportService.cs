using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Reports;
using WolfInvoice.Enums;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Models.DataModels;

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

        int customersCount = await _context.Customers.CountAsync();

        if (customersCount <= 0)
            return new();

        CustomerReport customerReport = new();

        var invoices = _context.Invoices
            .ToListAsync()
            .Result.FindAll(
                i =>
                    IsWithinPeriodStart(i.CreatedAt, period.Start)
                    && IsWithinPeriodEnd(i.CreatedAt, period.End)
            );

        if (invoices.Count <= 0)
            return new();

        decimal totalSum = invoices.Sum(i => i.TotalSum);

        customerReport.AverageCostPerCustomer = (totalSum / customersCount);

        customerReport.AverageInvoicePerCustomer = invoices.Count / (decimal)customersCount;

        customerReport.AverageInvoicePricePerCustomer = totalSum / invoices.Count;

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

        var invoices = _context.Invoices
            .ToListAsync()
            .Result.FindAll(
                i =>
                    IsWithinPeriodStart(i.CreatedAt, period.Start)
                    && IsWithinPeriodEnd(i.CreatedAt, period.End)
            );

        decimal invoicesCost = invoices.Sum(i => i.TotalSum);
        int invoiceCount = invoices.Count;

        if (invoiceCount <= 0)
            return new();

        report.TotalCost = invoicesCost;
        report.TotalInvoiceCount = invoiceCount;
        report.AveragePrice = invoicesCost / invoiceCount;

        return report;
    }

    private bool IsWithinPeriodStart(DateTimeOffset dateToCheck, DateTimeOffset? periodStart)
    {
        if (periodStart is null)
            return true;

        return dateToCheck >= periodStart;
    }

    private bool IsWithinPeriodEnd(DateTimeOffset dateToCheck, DateTimeOffset? periodEnd)
    {
        if (periodEnd == null)
            return true;

        return dateToCheck <= periodEnd;
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
