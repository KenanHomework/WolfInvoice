using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Invoice;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Enums;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Interfaces.EntityServices;

/// <summary>
/// Interface for <see cref="Invoice"/> service operations.
/// </summary>
public interface IInvoiceService
{
    /// <summary>
    /// Gets a paginated list of invoices.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="paginationRequest">The pagination request parameters.</param>
    /// <param name="filter">The query filters for getting the list.</param>
    /// <returns>A paginated list of invoices.</returns>
    public Task<PaginatedListDto<InvoiceDto>> GetInvoices(
        string userId,
        CreatePaginationRequest paginationRequest,
        GetListQueryFilter filter
    );

    /// <summary>
    /// Gets a invoice by the given id.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <returns>The <see cref="Invoice"/> object, or <see langword="null"/> if the invoice was not found.</returns>
    public Task<InvoiceDto?> GetInvoice(string userId, string invoiceId);

    /// <summary>
    /// Creates a new Invoice with the given id and request data.
    /// </summary>
    /// <param name="userId"> The ID of the user which is associated invoice.</param>
    /// <param name="request">The Invoice data to create.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <returns>The newly created Invoice.</returns>
    public Task<InvoiceDto> CreateInvoice(string userId, CreateInvoiceRequest request);

    /// <summary>
    /// Changes the status of an invoice.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <param name="status">The new status to set.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityProblemException"/>
    /// <returns>The updated invoice.</returns>
    public Task<InvoiceDto> ChangeInvoiceStatus(
        string userId,
        string invoiceId,
        InvoiceStatus status
    );

    /// <summary>
    /// Archives the Invoice with the given id.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <returns><see langword="true"/> if the Invoice was archived, <see langword="true"/> otherwise.</returns>
    public Task<bool> ArchiveInvoice(string userId, string invoiceId);

    /// <summary>
    /// Updates an existing Invoice with the given id and request data.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <param name="request">The Invoice data to update.</param>
    /// <returns>The updated Invoice.</returns>
    /// <exception cref="EntityNotFoundException"/>
    public Task<InvoiceDto> EditInvoice(
        string userId,
        string invoiceId,
        EditInvoiceRequest request
    );

    /// <summary>
    /// Deletes the Invoice with the given id.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityProblemException"/>
    /// <returns>True if the Invoice was deleted, false otherwise.</returns>
    public Task<bool> DeleteInvoice(string userId, string invoiceId);

    /// <summary>
    /// Adds a row to an invoice.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <param name="request">The request parameters for creating the row.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <returns>The updated invoice.</returns>
    public Task<InvoiceRowDto> AddRow(
        string userId,
        string invoiceId,
        CreateInvoiceRowRequest request
    );

    /// <summary>
    /// Adds a row to an invoice.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="invoiceId">The ID of the invoice to add the row to.</param>
    /// <param name="request">The request parameters for creating the rows.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <returns>The updated invoice.</returns>
    public Task<HashSet<InvoiceRowDto>> AddRowRange(
        string userId,
        string invoiceId,
        IEnumerable<CreateInvoiceRowRequest> request
    );

    /// <summary>
    /// Delete a row from an invoice.
    /// </summary>
    /// <param name="userId">The ID of the User.</param>
    /// <param name="invoiceId">The ID of the invoice to delete the row from</param>
    /// <param name="rowId">ID of the row</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <returns></returns>
    public Task<InvoiceRowDto> DeleteRow(string userId, string invoiceId, string rowId);

    /// <summary>
    /// Updates the invoice information according to the invoice row values in the given invoice object
    /// </summary>
    /// <param name="invoice">Invoice to be updated</param>
    public void CalculateInvoice(ref Invoice invoice);

    /// <summary>
    /// Checks if a <see cref="Invoice"/> with the given ID exists
    /// </summary>
    /// <param name="invoiceId">The ID of the <see cref="Invoice"/> to check</param>
    /// <param name="userId">The ID of the <see cref="User"/> to check</param>
    /// <returns><see langword="true"/> if the <see cref="Invoice"/> exists, <see langword="false"/> otherwise</returns>
    public Task<bool> InvoiceExistsById(string userId, string invoiceId);

    /// <summary>
    /// Converts a <see cref="Invoice"/> entity to a <see cref="InvoiceDto"/> data transfer object.
    /// </summary>
    /// <param name="invoice">The <see cref="Invoice"/> entity to convert.</param>
    /// <returns> The converted <see cref="InvoiceDto"/> object, or null if the input is null.</returns>
    public InvoiceDto? ConvertToDto(Invoice? invoice);
}
