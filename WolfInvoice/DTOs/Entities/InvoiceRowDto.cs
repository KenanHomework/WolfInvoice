using WolfInvoice.Models.DataModels;

namespace WolfInvoice.DTOs.Entities;

/// <summary>
/// Represents a invoice row data transfer object.
/// </summary>
public class InvoiceRowDto
{
    /// <summary>
    /// Initialize instance
    /// </summary>
    public InvoiceRowDto() { }

    /// <summary>
    /// Initialize instance
    /// </summary>
    /// <param name="invoiceRow"></param>
    public InvoiceRowDto(InvoiceRow invoiceRow)
    {
        Id = invoiceRow.Id;
        Service = invoiceRow.Service;
        Quantity = invoiceRow.Quantity;
        Amount = invoiceRow.Amount;
        Sum = invoiceRow.Sum;
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
}
