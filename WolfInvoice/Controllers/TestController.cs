using Microsoft.AspNetCore.Mvc;
using WolfInvoice.Data;
using WolfInvoice.Models.DataModels;
using WolfInvoice.Services;

namespace WolfInvoice.Controllers;

/// <summary>
///
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly WolfInvoiceContext _context;

    public TestController(WolfInvoiceContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a paginated list of Invoices filtered by the specified query parameters
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<User> Test()
    {
        LogService.LogReportAction("salam", Enums.ReportActions.CustomersReport);
        return Ok("everything is okay");
    }
}
