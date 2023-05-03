using Microsoft.EntityFrameworkCore.Query.Internal;
using WolfInvoice.Enums;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.DTOs.Entities;

/// <summary>
/// Represents a invoice row data transfer object.
/// </summary>
public class InvoiceDto
{
    /// <summary>
    /// Initialize instance
    /// </summary>
    public InvoiceDto() { }

    /// <summary>
    /// Initialize instance
    /// </summary>
    /// <param name="invoice"></param>
    public InvoiceDto(Invoice invoice)
    {
        Id = invoice.Id;
        UserId = invoice.User.Id;
        CustomerId = invoice.Customer.Id;
        StartDate = invoice.StartDate;
        EndDate = invoice.EndDate;
        TotalSum = invoice.TotalSum;
        Discount = invoice.Discount;
        Comment = invoice.Comment;
        Status = invoice.Status;
        Rows = invoice.Rows.Select(r => new InvoiceRowDto(r)).ToHashSet();
    }

    /// <summary>
    /// The unique identifier of the invoice.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the customer.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

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
    /// The rows of the invoice containing the details of each item.
    /// </summary>
    public HashSet<InvoiceRowDto> Rows { get; set; } = new();
}
