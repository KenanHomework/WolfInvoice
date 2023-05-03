using System.Text.Json.Serialization;

namespace WolfInvoice.Enums;

/// <summary>
/// Represents the status of an invoice.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InvoiceStatus
{
    /// <summary>
    /// The invoice has been created but not sent or received.
    /// </summary>
    Created,

    /// <summary>
    /// The invoice has been sent to the customer.
    /// </summary>
    Sent,

    /// <summary>
    /// The invoice has been received from the provider.
    /// </summary>
    Received,

    /// <summary>
    /// The invoice has been paid.
    /// </summary>
    Paid,

    /// <summary>
    /// The invoice has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The invoice has been rejected.
    /// </summary>
    Rejected
}
