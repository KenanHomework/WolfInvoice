namespace WolfInvoice.DTOs.Requests.Invoice;

/// <summary>
/// Invoice Row create request DTO
/// </summary>
public class CreateInvoiceRowRequest
{
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
}
