using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using WolfInvoice.Configurations;
using WolfInvoice.Data;
using WolfInvoice.Documents;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Filters;
using WolfInvoice.DTOs.Pagination;
using WolfInvoice.DTOs.Requests.Invoice;
using WolfInvoice.DTOs.Requests.Pagination;
using WolfInvoice.Enums;
using WolfInvoice.Exceptions.EntityExceptions;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Interfaces.IProviders;
using WolfInvoice.Models.DataModels;

namespace WolfInvoice.Controllers;

/// <summary>
/// Controller for handling invoice management
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly WolfInvoiceContext _context;
    private readonly CompanyInfoConfig _companyInfo;
    private readonly IInvoiceService _invoiceService;
    private readonly IRequestUserProvider _userProvider;

    /// <summary>
    ///  Initialize object
    /// </summary>
    /// <param name="context"></param>
    /// <param name="invoiceService"></param>
    /// <param name="userProvider"></param>
    /// <param name="companyInfo"></param>
    public InvoicesController(
        WolfInvoiceContext context,
        IInvoiceService invoiceService,
        IRequestUserProvider userProvider,
        CompanyInfoConfig companyInfo
    )
    {
        _context = context;
        _invoiceService = invoiceService;
        _userProvider = userProvider;
        _companyInfo = companyInfo;
    }

    /// <summary> Retrieves a paginated list of Invoices filtered by the specified query parameters. </summary>
    /// <param name="paginationRequest"> Pagination parameters (page number, page size). </param>
    /// <param name="queryFilter">
    /// Filtering parameters (search query, sorting direction and field).
    /// </param>
    /// <returns>
    /// An <see cref="ActionResult" /> of a <see cref="PaginatedListDto{InvoiceDto}" /> that
    /// contains the list of Invoices.
    /// </returns>
    // GET: api/Invoices
    [HttpGet]
    public async Task<ActionResult<PaginatedListDto<InvoiceDto>>> GetInvoices(
        [FromQuery] CreatePaginationRequest paginationRequest,
        [FromQuery] GetListQueryFilter queryFilter
    )
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        PaginatedListDto<InvoiceDto> list;

        try
        {
            list = await _invoiceService.GetInvoices(userId, paginationRequest, queryFilter);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(list);
    }

    /// <summary>
    /// Get a invoice by id
    /// </summary>
    /// <param name="invoiceId">The id of the user to get</param>
    ///             <returns>The requested user</returns>
    // GET: api/invoices/5
    [HttpGet("{invoiceId}")]
    public async Task<ActionResult<InvoiceDto>> GetInvoice(string invoiceId)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.GetInvoice(userId, invoiceId);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        if (invoice is null)
            return NotFound();

        return Ok(invoice);
    }

    /// <summary>
    /// Downloads an invoice as a PDF file.
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice to download.</param>
    /// <returns>An IActionResult representing the downloaded file.</returns>
    [HttpGet("{invoiceId}/download")]
    public async Task<IActionResult> DownloadInvoiceAsPdf(string invoiceId)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        Invoice? invoice;

        try
        {
            invoice = await _context.Invoices
                .Include(i => i.User)
                .Include(i => i.Customer)
                .Include(i => i.Rows)
                .FirstOrDefaultAsync(i => i.Id.Equals(invoiceId) && i.User.Id.Equals(userId));

            if (invoice is null)
                return NotFound();

            var document = new InvoiceDocument(invoice, _companyInfo);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string pdfFilePath = Path.Combine(desktopPath, $"WolfInvoice_invoice_{invoice.Id}.pdf");

            document.GeneratePdf(pdfFilePath);

            return Ok(File(document.GeneratePdf(), "application/pdf"));
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }
    }

    /// <summary>
    /// Creates a new invoice with the given id and request data.
    /// </summary>
    /// <param name="request">The invoice data to create.</param>
    /// <returns>The newly created invoice.</returns>
    // POST: api/invoices/5
    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceRequest request)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.CreateInvoice(userId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Creates a new invoice row for a given invoice
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice to add the row to</param>
    /// <param name="request">The request object containing the row details</param>
    /// <returns>The updated invoice object</returns>
    [HttpPost("{invoiceId}/createRow")]
    public async Task<ActionResult<InvoiceDto>> CreateInvoiceRow(
        string invoiceId,
        CreateInvoiceRowRequest request
    )
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.AddRow(userId, invoiceId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Creates multiple invoice rows for a given invoice
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice to add the rows to</param>
    /// <param name="request">An enumerable of request objects containing the row details</param>
    /// <returns>The updated invoice object</returns>
    [HttpPost("{invoiceId}/createRowRange")]
    public async Task<ActionResult<InvoiceDto>> CreateInvoiceRowRange(
        string invoiceId,
        IEnumerable<CreateInvoiceRowRequest> request
    )
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.AddRowRange(userId, invoiceId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Deletes an invoice row from a given invoice
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice to delete the row from</param>
    /// <param name="rowId">The ID of the row to delete</param>
    /// <returns>The updated invoice object</returns>
    [HttpDelete("{invoiceId}/deleteRow")]
    public async Task<ActionResult<InvoiceDto>> DeleteInvoiceRow(string invoiceId, string rowId)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.DeleteRow(userId, invoiceId, rowId);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(invoice);
    }

    /// <summary>
    /// Archive a invoice
    /// </summary>
    /// <param name="invoiceId">The id of the user to get</param>
    /// <returns>A result indicating success or failure</returns>
    // PATCH: api/invoices/5
    [HttpDelete("{invoiceId}/archive")]
    public async Task<IActionResult> ArchiveInvoice(string invoiceId)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        try
        {
            var condition = await _invoiceService.ArchiveInvoice(userId, invoiceId);

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
    /// Changes the status of an invoice.
    /// </summary>
    /// <param name="invoiceId">The ID of the Invoice.</param>
    /// <param name="status">The new status to set.</param>
    /// <returns>The updated invoice.</returns>
    // PATCH: api/invoices/5
    [HttpPatch]
    public async Task<ActionResult<InvoiceDto>> ChangeInvoiceStatus(
        string invoiceId,
        InvoiceStatus status
    )
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.ChangeInvoiceStatus(userId, invoiceId, status);
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
            throw;
        }

        return Ok();
    }

    /// <summary>
    /// Update a invoice
    /// </summary>
    /// <param name="invoiceId">The id of the user to get</param>
    /// <param name="request">The request</param>
    /// <returns>A result indicating success or failure</returns>
    // PUT: api/invoices/5
    [HttpPut("{invoiceId}")]
    public async Task<ActionResult<InvoiceDto>> UpdateInvoice(
        string invoiceId,
        EditInvoiceRequest request
    )
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        InvoiceDto? invoice;

        try
        {
            invoice = await _invoiceService.EditInvoice(userId, invoiceId, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        if (invoice is null)
            return NotFound();

        return Ok(invoice!);
    }

    /// <summary>
    /// Delete a invoice by id
    /// </summary>
    /// <returns>A result indicating success or failure</returns>
    // DELETE: api/invoices/5
    [HttpDelete("{invoiceId}")]
    public async Task<IActionResult> DeleteInvoice(string invoiceId)
    {
        if (_context.Invoices is null)
            return NotFound();

        var userId = _userProvider.GetUserInfo()!.Id;

        try
        {
            var condition = await _invoiceService.DeleteInvoice(userId, invoiceId);

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
