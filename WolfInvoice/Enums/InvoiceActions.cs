using System.Text.Json.Serialization;

namespace WolfInvoice.Enums;

/// <summary>
/// Represents the various actions that can be performed on an invoice.
/// </summary>
[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
public enum InvoiceActions
{
    /// <summary>
    /// The invoice was created.
    /// </summary>
    Created,

    /// <summary>
    /// The invoice was edited.
    /// </summary>
    Edited,

    /// <summary>
    /// The invoice was sent to the customer.
    /// </summary>
    Sent,

    /// <summary>
    /// The customer received the invoice.
    /// </summary>
    Received,

    /// <summary>
    /// The invoice was paid by the customer.
    /// </summary>
    Paid,

    /// <summary>
    /// The invoice was canceled by the user.
    /// </summary>
    Canceled,

    /// <summary>
    /// The invoice was rejected by the customer.
    /// </summary>
    Rejected,

    /// <summary>
    /// The invoice was archived.
    /// </summary>
    Archived,

    /// <summary>
    /// The invoice was deleted.
    /// </summary>
    Deleted,

    /// <summary>
    /// The invoice rows was added.
    /// </summary>
    RowAdded,

    /// <summary>
    /// The invoice rows was added range.
    /// </summary>
    RowRangeAdded,

    /// <summary>
    /// The invoice rows was deleted.
    /// </summary>
    RowDeleted
}
