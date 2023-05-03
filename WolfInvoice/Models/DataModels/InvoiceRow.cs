using Microsoft.EntityFrameworkCore.Query.Internal;
using WolfInvoice.DTOs.Requests.Invoice;
using WolfInvoice.Services;

namespace WolfInvoice.Models.DataModels;

/// <summary>
/// Represents a single row in an invoice.
/// </summary>
public class InvoiceRow
{
    /// <summary>
    /// Initialize instance
    /// </summary>
    public InvoiceRow() { }

    /// <summary>
    /// Initialize instance
    /// </summary>
    /// <param name="invoice"></param>
    /// <param name="request"></param>
    public InvoiceRow(Invoice invoice, CreateInvoiceRowRequest request)
    {
        Id = IdGeneratorService.GetUniqueId();
        Invoice = invoice;
        Amount = request.Amount;
        Quantity = request.Quantity;
        Service = request.Service;
        Sum = request.Quantity * request.Amount;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the invoice row.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the service provided in the invoice row.
    /// </summary>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the service provided in the invoice row.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the amount charged for each unit of service in the invoice row.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the total sum charged for the service provided in the invoice row.
    /// </summary>
    public decimal Sum { get; set; }

    /* Navigation Properties */

    /// <summary>
    /// Gets or sets the invoice that the row belongs to.
    /// </summary>
    public Invoice Invoice { get; set; } = default!;
}
