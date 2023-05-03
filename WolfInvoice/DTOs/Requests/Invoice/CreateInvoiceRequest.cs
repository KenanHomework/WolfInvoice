namespace WolfInvoice.DTOs.Requests.Invoice;

/// <summary>
/// Create Invoice request DTO
/// </summary>
public class CreateInvoiceRequest
{
    /// <summary>
    /// The ID of the customer which is associated invoice.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// The discount percent of the invoice.
    /// </summary>
    public decimal? Discount { get; set; }

    /// <summary>
    /// The comment associated with the invoice.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The rows of the invoice containing the details of each item.
    /// </summary>
    public ICollection<CreateInvoiceRowRequest>? Rows { get; set; }
}
