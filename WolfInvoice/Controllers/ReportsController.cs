using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Reports;
using WolfInvoice.Enums;
using WolfInvoice.Interfaces.EntityServices;

namespace WolfInvoice.Controllers;

/// <summary>
/// Controller for handling report management
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly WolfInvoiceContext _context;
    private readonly IReportService _reportService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportsController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="reportService">The report service.</param>
    public ReportsController(WolfInvoiceContext context, IReportService reportService)
    {
        _context = context;
        _reportService = reportService;
    }

    /// <summary>
    /// Gets the customer report for a specified time period.
    /// </summary>
    /// <param name="period">The time period.</param>
    /// <returns>The customer report.</returns>
    [HttpGet("customer")]
    public async Task<ActionResult<CustomerReport>> GetCustomerReport([FromQuery] TimePeriod period)
    {
        CustomerReport report;
        try
        {
            report = await _reportService.GetCustomerReport(period);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(report);
    }

    /// <summary>
    /// Gets the invoice report for a specified time period and status.
    /// </summary>
    /// <param name="period">The time period.</param>
    /// <param name="status">The invoice status.</param>
    /// <returns>The invoice report.</returns>
    [HttpGet("invoice")]
    public async Task<ActionResult<InvoiceReport>> GetInvoiceReport(
        [FromQuery] TimePeriod period,
        [FromQuery] InvoiceStatus? status
    )
    {
        InvoiceReport report;
        try
        {
            report = await _reportService.GetInvoiceReport(period, status);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(report);
    }

    /// <summary>
    /// Gets the invoice by status report for a specified time period.
    /// </summary>
    /// <param name="period">The time period.</param>
    /// <returns>The invoice by status report.</returns>
    [HttpGet("invoicebystatus")]
    public async Task<ActionResult<InvoiceByStatusReport>> GetInvoiceByStatusReport(
        [FromQuery] TimePeriod period
    )
    {
        InvoiceByStatusReport report;
        try
        {
            report = await _reportService.GetInvoiceByStatusReport(period);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(report);
    }
}
