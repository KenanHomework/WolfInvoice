using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Customer;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Interfaces.EntityServices;

/// <summary>
/// Interface for <see cref="Customer"/> service operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Gets a paginated list of customers.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="paginationRequest">The pagination request parameters.</param>
    /// <param name="filter">The query filters for getting the list.</param>
    /// <returns>A paginated list of customers.</returns>
    public Task<PaginatedListDto<CustomerDto>> GetCustomers(
        string userId,
        CreatePaginationRequest paginationRequest,
        GetListQueryFilter filter
    );

    /// <summary>
    /// Gets a customer by the given id.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="customerId">The customer id.</param>
    /// <returns>The <see cref="Customer"/> object, or <see langword="null"/> if the customer was not found.</returns>
    public Task<CustomerDto?> GetCustomer(string userId, string customerId);

    /// <summary>
    /// Creates a new customer with the given id and request data.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="request">The customer data to create.</param>
    /// <returns>The newly created customer.</returns>
    /// <exception cref="EntityConflictException"/>
    /// <exception cref="EntityNotFoundException"/>
    public Task<CustomerDto> CreateCustomer(string userId, CreateCustomerRequest request);

    /// <summary>
    /// Updates an existing customer with the given id and request data.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="customerId">The customer id.</param>
    /// <param name="request">The customer data to update.</param>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityConflictException"/>
    /// <returns>The updated customer.</returns>
    public Task<CustomerDto> EditCustomer(
        string userId,
        string customerId,
        EditCustomerRequest request
    );

    /// <summary>
    /// Archives the customer with the given id.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="customerId">The customer id.</param>
    /// <returns><see langword="true"/> if the customer was archived, <see langword="true"/> otherwise.</returns>
    /// <exception cref="EntityNotFoundException"/>
    public Task<bool> ArchiveCustomer(string userId, string customerId);

    /// <summary>
    /// Deletes the customer with the given id.
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="customerId">The customer id.</param>
    /// <returns>True if the customer was deleted, false otherwise.</returns>
    /// <exception cref="EntityNotFoundException"/>
    /// <exception cref="EntityProblemException"/>
    public Task<bool> DeleteCustomer(string userId, string customerId);

    /// <summary>
    /// Checks if a <see cref="Customer"/> with the given ID exists
    /// </summary>
    /// <param name="customerId">The ID of the <see cref="Customer"/> to check</param>
    /// <param name="userId">The ID of the <see cref="User"/> to check</param>
    /// <returns><see langword="true"/> if the <see cref="Customer"/> exists, <see langword="false"/> otherwise</returns>
    public Task<bool> CustomerExistsById(string customerId, string userId);

    /// <summary>
    /// Checks if a <see cref="User"/> with the given Email address exists
    /// </summary>
    /// <param name="email">The ID of the <see cref="User"/> to check</param>
    /// <param name="userId">The ID of the <see cref="User"/> to check</param>
    /// <returns><see langword="true"/> if the <see cref="User"/> exists, <see langword="false"/> otherwise</returns>
    public Task<bool> CustomerExistsByEmail(string email, string? userId = null);

    /// <summary>
    /// Converts a <see cref="Customer"/> entity to a <see cref="CustomerDto"/> data transfer object.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> entity to convert.</param>
    /// <returns> The converted <see cref="CustomerDto"/> object, or null if the input is null.</returns>
    public CustomerDto? ConvertToDto(Customer customer);
}
