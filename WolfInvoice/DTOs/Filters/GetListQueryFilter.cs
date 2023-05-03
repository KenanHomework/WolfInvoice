using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using WolfInvoice.Controllers;
using WolfInvoice.Enums;

namespace WolfInvoice.DTOs.Filters;

/// <summary>
/// Represents query filters for getting a list.
/// </summary>
public class GetListQueryFilter
{
    /// <summary>
    /// Gets or sets the search input for filtering the list.
    /// </summary>
    [FromQuery(Name = "searchInput")]
    public string? SearchInput { get; set; }

    /// <summary>
    /// Gets or sets the invoice status for filtering the list.
    /// </summary>
    [FromQuery(Name = "invoiceStatus")]
    public InvoiceStatus? InvoiceStatus { get; set; }

    /// <summary>
    /// Gets or sets the entity status for filtering the list.
    /// </summary>
    [FromQuery(Name = "entityStatus")]
    public EntityStatus? EntityStatus { get; set; }

    /// <summary>
    /// Gets or sets the sorting criteria for sorting the list.
    /// </summary>
    [FromQuery(Name = "sorting")]
    public SortingSide? Sorting { get; set; } = SortingSide.Ascending;
}
