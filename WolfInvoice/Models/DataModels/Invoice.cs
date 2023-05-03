using WolfInvoice.Enums;

namespace WolfInvoice.Models.DataModels;

/// <summary>
/// Represents an invoice in the system.
/// </summary>
public class Invoice
{
    /// <summary>
    /// The unique identifier of the invoice.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The start date of the invoice.
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// The end date of the invoice.
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// The total sum of the invoice.
    /// </summary>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// The discount percent of the invoice.
    /// </summary>
    public decimal? Discount { get; set; }

    /// <summary>
    /// The comment associated with the invoice.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The progress status of the invoice.
    /// </summary>
    public InvoiceStatus Status { get; set; }

    /// <summary>
    /// Represents the life status of invoice
    /// </summary>
    public EntityStatus EntityStatus { get; set; } = EntityStatus.Active;

    /// <summary>
    /// The creation date and time of the invoice.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The last update date and time of the invoice.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// The deletion date and time of the invoice.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /* Navigation Properties */

    /// <summary>
    /// The unique identifier of the user associated with the invoice.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    /// The unique identifier of the customer associated with the invoice.
    /// </summary>
    public Customer Customer { get; set; } = default!;

    /// <summary>
    /// The rows of the invoice containing the details of each item.
    /// </summary>
    public HashSet<InvoiceRow> Rows { get; set; } = new();
}
