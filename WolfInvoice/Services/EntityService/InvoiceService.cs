using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Invoice;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Enums;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Services.EntityService;

/// <summary>
/// Service for <see cref="Invoice"/> service operations.
/// </summary>
public class InvoiceService : IInvoiceService
{
    private readonly WolfInvoiceContext _context;
    private readonly IUserService _userService;

    /// <summary>
    /// Initialize instance
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userService"></param>
    public InvoiceService(WolfInvoiceContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    /// <inheritdoc/>
    public async Task<PaginatedListDto<InvoiceDto>> GetInvoices(
        string userId,
        CreatePaginationRequest paginationRequest,
        GetListQueryFilter filter
    )
    {
        IQueryable<Invoice> query = _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Customer)
            .Where(c => c.User.Id.Equals(userId) && c.EntityStatus == EntityStatus.Active);

        if (!string.IsNullOrWhiteSpace(filter.SearchInput))
            query = query.Where(i => i.Rows.Any(r => r.Service.Contains(filter.SearchInput)));

        switch (filter.Sorting)
        {
            case SortingSide.Ascending:
                query = query.OrderBy(i => (double)i.TotalSum);

                break;
            case SortingSide.Descending:
                query = query.OrderByDescending(i => (double)i.TotalSum);
                break;
        }

        var totalCount = await query.CountAsync();
        var invoices = await query
            .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync();

        return new PaginatedListDto<InvoiceDto>(
            invoices.Select(t => new InvoiceDto(t)),
            new PaginationMeta(paginationRequest.Page, paginationRequest.PageSize, totalCount)
        );
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto?> GetInvoice(string userId, string invoiceId) =>
        ConvertToDto(await GetPrivateInvoice(userId, invoiceId));

    /// <inheritdoc/>
    public async Task<InvoiceDto> CreateInvoice(string userId, CreateInvoiceRequest request)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId))
            ?? throw new EntityNotFoundException("User not found of given id!");

        var customer =
            await _context.Customers.FirstOrDefaultAsync(
                c => c.Id.Equals(request.CustomerId) && c.User.Id.Equals(userId)
            ) ?? throw new EntityNotFoundException("Customer not found of given id!");

        var invoice = new Invoice()
        {
            Id = IdGeneratorService.GetShortUniqueId(),
            User = user,
            Customer = customer,
            Comment = request.Comment,
            Discount = request.Discount,
            EntityStatus = EntityStatus.Active,
            Status = InvoiceStatus.Created,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        if (request.Rows is not null && request.Rows.Count > 0)
            invoice.Rows = request.Rows.Select(r => new InvoiceRow(invoice, r)).ToHashSet();

        CalculateInvoice(ref invoice);

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoice.Id, user.Id, InvoiceActions.Created);

        return new InvoiceDto(invoice);
    }

    /// <inheritdoc/>
    public async Task<bool> ArchiveInvoice(string userId, string invoiceId)
    {
        if (!await _userService.UserExistsById(userId))
            throw new EntityNotFoundException("User not found of given id!");

        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        invoice.EntityStatus = EntityStatus.Archived;

        invoice.UpdatedAt = DateTime.UtcNow;

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.Archived);

        return true;
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> ChangeInvoiceStatus(
        string userId,
        string invoiceId,
        InvoiceStatus status
    )
    {
        if (!await _userService.UserExistsById(userId))
            throw new EntityNotFoundException("User not found of given id!");

        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        switch (status)
        {
            case InvoiceStatus.Created:
                throw new EntityProblemException(
                    "You can't update the status of an already created invoice in created!"
                );
            case InvoiceStatus.Received:
                invoice.Status = InvoiceStatus.Received;
                invoice.StartDate = DateTimeOffset.Now;
                break;
            case InvoiceStatus.Paid:
                invoice.Status = InvoiceStatus.Paid;
                invoice.EndDate = DateTimeOffset.Now;
                break;
            default:
                invoice.Status = status;
                break;
        }

        invoice.UpdatedAt = DateTime.UtcNow;

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, status);

        return new InvoiceDto(invoice);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> EditInvoice(
        string userId,
        string invoiceId,
        EditInvoiceRequest request
    )
    {
        if (!await _userService.UserExistsById(userId))
            throw new EntityNotFoundException("User not found of given id!");

        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        invoice.Comment = request.Comment ?? invoice.Comment;
        invoice.Discount = request.Discount ?? invoice.Discount;

        invoice.UpdatedAt = DateTime.UtcNow;

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.Edited);

        return new(invoice);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteInvoice(string userId, string invoiceId)
    {
        if (!await _userService.UserExistsById(userId))
            throw new EntityNotFoundException("User not found of given id!");

        var invoice =
            await GetPrivateInvoiceWithoutEntityStatus(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        if (invoice.Status is InvoiceStatus.Sent or InvoiceStatus.Paid or InvoiceStatus.Received)
            throw new EntityProblemException(
                "This invoice is already in process or finished, you cannot delete this invoice but you can archive it"
            );

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.Deleted);

        return true;
    }

    /// <inheritdoc/>
    public void CalculateInvoice(ref Invoice invoice)
    {
        decimal totalSum = 0;

        foreach (var row in invoice.Rows)
            totalSum += row.Sum;

        totalSum -= ((totalSum / 100) * invoice.Discount ?? 0);

        invoice.TotalSum = totalSum;
    }

    /// <inheritdoc/>
    public async Task<bool> InvoiceExistsById(string invoiceId, string userId) =>
        await _context.Invoices.AnyAsync(i => i.Id == invoiceId && i.User.Id == userId);

    /// <inheritdoc/>
    public InvoiceDto? ConvertToDto(Invoice? invoice) =>
        invoice is null ? null : new InvoiceDto(invoice);

    /* Invoice Row */

    /// <inheritdoc/>
    public async Task<InvoiceDto> AddRow(
        string userId,
        string invoiceId,
        CreateInvoiceRowRequest request
    )
    {
        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        var row = new InvoiceRow(invoice, request);

        invoice.Rows.Add(row);

        CalculateInvoice(ref invoice);

        invoice.UpdatedAt = DateTime.UtcNow;

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.RowAdded);

        return new InvoiceDto(invoice);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> AddRowRange(
        string userId,
        string invoiceId,
        IEnumerable<CreateInvoiceRowRequest> request
    )
    {
        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        var rows = request.Select(r => new InvoiceRow(invoice, r));

        foreach (var row in rows)
            invoice.Rows.Add(row);

        invoice.UpdatedAt = DateTime.UtcNow;

        CalculateInvoice(ref invoice);

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.RowRangeAdded);

        return new InvoiceDto(invoice);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> DeleteRow(string userId, string invoiceId, string rowId)
    {
        var invoice =
            await GetPrivateInvoice(userId, invoiceId)
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        var row =
            invoice.Rows.FirstOrDefault(r => r.Id.Equals(rowId))
            ?? throw new EntityNotFoundException("Invoice not found of given id!");

        invoice.Rows.RemoveWhere(r => r.Id.Equals(rowId));

        invoice.UpdatedAt = DateTime.UtcNow;

        CalculateInvoice(ref invoice);

        _context.Update(invoice);
        await _context.SaveChangesAsync();

        LogService.LogInvoiceAction(invoiceId, userId, InvoiceActions.RowDeleted);

        return new InvoiceDto(invoice);
    }

    private async Task<Invoice?> GetPrivateInvoice(string userId, string invoiceId) =>
        await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(
                i =>
                    i.Id.Equals(invoiceId)
                    && i.User.Id.Equals(userId)
                    && i.EntityStatus == EntityStatus.Active
            );

    private async Task<Invoice?> GetPrivateInvoiceWithoutEntityStatus(
        string userId,
        string invoiceId
    ) =>
        await _context.Invoices
            .Include(i => i.User)
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id.Equals(invoiceId) && i.User.Id.Equals(userId));
}
