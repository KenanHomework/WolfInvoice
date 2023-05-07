using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Customer;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Enums;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Services.EntityService;

/// <summary>
/// Service for <see cref="Customer"/> service operations.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly WolfInvoiceContext _context;

    /// <summary>
    /// Initialize object
    /// </summary>
    /// <param name="context"></param>
    public CustomerService(WolfInvoiceContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<PaginatedListDto<CustomerDto>> GetCustomers(
        string userId,
        CreatePaginationRequest paginationRequest,
        GetListQueryFilter filter
    )
    {
        IQueryable<Customer> query = _context.Customers.Where(
            c => c.User.Id.Equals(userId) && c.EntityStatus == EntityStatus.Active
        );

        if (!string.IsNullOrWhiteSpace(filter.SearchInput))
            query = query.Where(c => c.Name.Contains(filter.SearchInput));

        switch (filter.Sorting)
        {
            case SortingSide.Ascending:
                query = query.OrderBy(c => c.Name);

                break;
            case SortingSide.Descending:
                query = query.OrderByDescending(c => c.Name);
                break;
        }

        var totalCount = await query.CountAsync();
        var customers = await query
            .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync();

        return new PaginatedListDto<CustomerDto>(
            customers.Select(t => new CustomerDto(t)),
            new PaginationMeta(paginationRequest.Page, paginationRequest.PageSize, totalCount)
        );
    }

    /// <inheritdoc/>
    public async Task<CustomerDto?> GetCustomer(string userId, string customerId) =>
        ConvertToDto(
            await (
                from u in _context.Users
                join c in _context.Customers on u.Id equals c.User.Id
                where
                    u.Id.Equals(userId)
                    && c.Id.Equals(customerId)
                    && c.EntityStatus == EntityStatus.Active
                select c
            ).FirstOrDefaultAsync()
        );

    /// <inheritdoc/>
    public async Task<CustomerDto> CreateCustomer(string userId, CreateCustomerRequest request)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId))
            ?? throw new EntityNotFoundException("User not found of given id!");

        var customer = await (
            from u in _context.Users
            join c in _context.Customers on u.Id equals c.User.Id
            where u.Id.Equals(userId) && c.Email.Equals(request.Email)
            select c
        ).FirstOrDefaultAsync();

        if (customer is not null)
            throw new EntityConflictException("There is already a customer with this email!");

        customer = new Customer()
        {
            Id = IdGeneratorService.GetUniqueId(),
            User = user,
            Name = request.Name,
            Address = request.Address,
            CreditCard = request.CreditCard,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        LogService.LogCustomerAction(customer.Id, userId, CustomerActions.Created);

        return new CustomerDto(customer);
    }

    /// <inheritdoc/>
    public async Task<bool> ArchiveCustomer(string userId, string customerId)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId))
            ?? throw new EntityNotFoundException("User not found of given id!");

        var customer =
            await (
                from u in _context.Users
                join c in _context.Customers on u.Id equals c.User.Id
                where
                    u.Id.Equals(userId)
                    && c.Id.Equals(customerId)
                    && c.EntityStatus == EntityStatus.Active
                select c
            ).FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException("Customer not found of given id!");

        customer.EntityStatus = EntityStatus.Archived;

        customer.UpdatedAt = DateTime.UtcNow;

        _context.Update(customer);
        await _context.SaveChangesAsync();

        LogService.LogCustomerAction(customerId, userId, CustomerActions.Archived);

        return true;
    }

    /// <inheritdoc/>
    public async Task<CustomerDto> EditCustomer(
        string userId,
        string customerId,
        EditCustomerRequest request
    )
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId))
            ?? throw new EntityNotFoundException("User not found of given id!");

        var customer =
            await (
                from u in _context.Users
                join c in _context.Customers on u.Id equals c.User.Id
                join i in _context.Invoices on u.Id equals i.User.Id
                where u.Id.Equals(userId) && c.Id.Equals(customerId)
                select c
            ).FirstOrDefaultAsync()
            ?? throw new EntityNotFoundException("Customer not found of given id!");

        customer.Name = request.Name ?? customer.Name;
        customer.PhoneNumber = request.PhoneNumber ?? customer.PhoneNumber;
        customer.Address = request.Address ?? customer.Address;
        customer.CreditCard = request.CreditCard ?? customer.CreditCard;

        if (!string.IsNullOrWhiteSpace(request.Email))
            customer.Email = await CustomerExistsByEmail(request.Email)
                ? throw new EntityConflictException("There is already a user with this email!")
                : request.Email;

        customer.UpdatedAt = DateTime.UtcNow;

        _context.Update(customer);
        await _context.SaveChangesAsync();

        LogService.LogCustomerAction(customerId, userId, CustomerActions.Edited);

        return new CustomerDto(customer);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteCustomer(string userId, string customerId)
    {
        var user =
            await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId))
            ?? throw new EntityNotFoundException("User not found of given id!");

        var customer =
            await _context.Customers
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id.Equals(customerId) && c.User.Id.Equals(userId))
            ?? throw new EntityNotFoundException("Customer not found of given id!");

        if (customer.Invoices.Count > 0)
            throw new EntityProblemException(
                "You can only delete customers that have never sent an Invoice"
            );

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        LogService.LogCustomerAction(customerId, userId, CustomerActions.Deleted);

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> CustomerExistsByEmail(string email, string? userId = null)
    {
        var query = _context.Customers.Where(c => c.Email == email);

        if (!string.IsNullOrWhiteSpace(userId))
            query = query.Where(c => c.User.Id != userId);

        return await query.AnyAsync();
    }

    /// <inheritdoc/>
    public Task<bool> CustomerExistsById(string customerId, string userId) =>
        _context.Customers.AnyAsync(c => c.Id == customerId && c.User.Id == userId);

    /// <inheritdoc/>
    public CustomerDto? ConvertToDto(Customer? customer) =>
        customer is null ? null : new CustomerDto(customer);
}
