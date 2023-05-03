using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Customer;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Interfaces.IProviders;

namespace WolfInvoice.Controllers;

/// <summary>
/// Controller for handling user management
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly WolfInvoiceContext _context;
    private readonly ICustomerService _customerService;
    private readonly IRequestUserProvider _userProvider;

    /// <summary>
    /// Initialize object
    /// </summary>
    /// <param name="context"></param>
    /// <param name="customerService"></param>
    /// <param name="userProvider"></param>S
    public CustomersController(
        WolfInvoiceContext context,
        ICustomerService customerService,
        IRequestUserProvider userProvider
    )
    {
        _context = context;
        _customerService = customerService;
        _userProvider = userProvider;
    }

    /// <summary> Retrieves a paginated list of customers filtered by the specified query parameters. </summary>
    /// <param name="paginationRequest"> Pagination parameters (page number, page size). </param>
    /// <param name="queryFilter">
    /// Filtering parameters (search query, sorting direction and field).
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult" /> of a <see cref="PaginatedListDto{CustomerDto}" /> that
    /// contains the list of customers.
    /// </returns>
    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<PaginatedListDto<CustomerDto>>> GetCustomers(
        [FromQuery] CreatePaginationRequest paginationRequest,
        [FromQuery] GetListQueryFilter queryFilter
    )
    {
        if (_context.Customers is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        PaginatedListDto<CustomerDto> list;

        try
        {
            list = await _customerService.GetCustomers(userId, paginationRequest, queryFilter);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(list);
    }

    /// <summary>
    /// Get a customer by id
    /// </summary>
    /// <param name="customerId">The id of the user to get</param>
    ///             <returns>The requested user</returns>
    // GET: api/Customers/5
    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(string customerId)
    {
        if (_context.Customers is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        CustomerDto? customer;

        try
        {
            customer = await _customerService.GetCustomer(userId, customerId);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    /// <summary>
    /// Creates a new customer with the given id and request data.
    /// </summary>
    /// <param name="request">The customer data to create.</param>
    /// <returns>The newly created customer.</returns>
    // POST: api/Users/5
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request)
    {
        if (_context.Customers is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        CustomerDto? customer;

        try
        {
            customer = await _customerService.CreateCustomer(userId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityConflictException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(customer);
    }

    /// <summary>
    /// Update a customer
    /// </summary>
    /// <param name="customerId">The id of the user to get</param>
    /// <param name="request">The request</param>
    /// <returns>A result indicating success or failure</returns>
    // PUT: api/Customers/5
    [HttpPut("{customerId}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(
        string customerId,
        EditCustomerRequest request
    )
    {
        if (_context.Customers is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        CustomerDto? customer;

        try
        {
            customer = await _customerService.EditCustomer(userId, customerId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityConflictException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        if (customer is null)
            return NotFound();

        return Ok(customer!);
    }

    /// <summary>
    /// Archive a customer
    /// </summary>
    /// <param name="customerId">The id of the user to get</param>
    /// <returns>A result indicating success or failure</returns>
    // PATCH: api/Users/5
    [HttpDelete("{customerId}/archive")]
    public async Task<IActionResult> ArchiveCustomer(string customerId)
    {
        if (_context.Customers is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        try
        {
            var condition = await _customerService.ArchiveCustomer(userId, customerId);

            if (!condition)
                return Problem("A problem has occurred please try again later");
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok();
    }

    /// <summary>
    /// Delete a customer by id
    /// </summary>
    /// <returns>A result indicating success or failure</returns>
    // DELETE: api/Customers/5
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        if (_context.Customers is null)
            return NotFound();

        var id = _userProvider.GetUserInfo()!.Id;

        try
        {
            var condition = await _customerService.DeleteCustomer(customerId, id);

            if (!condition)
                return BadRequest(
                    "There was a problem trying to delete a user, please try again later"
                );
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityProblemException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok();
    }
}
