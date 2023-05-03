namespace WolfInvoice.DTOs.Requests.Invoice;

/// <summary>
/// The update Invoice request DTO
/// </summary>
public class EditInvoiceRequest
{
    /// <summary>
    /// The discount percent of the invoice.
    /// </summary>
    public decimal? Discount { get; set; }

    /// <summary>
    /// The comment associated with the invoice.
    /// </summary>
    public string? Comment { get; set; }
}
