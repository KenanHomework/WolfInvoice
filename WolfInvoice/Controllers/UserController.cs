using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WolfInvoice.Data;
using WolfInvoice.DTOs.Entities;
using WolfInvoice.DTOs.Requests.User;
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
public class UserController : ControllerBase
{
    private readonly WolfInvoiceContext _context;
    private readonly IRequestUserProvider _userProvider;
    private readonly IUserService _userService;

    /// <summary>
    /// Initialize object
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userProvider"></param>
    /// <param name="userService"></param>
    public UserController(
        WolfInvoiceContext context,
        IRequestUserProvider userProvider,
        IUserService userService
    )
    {
        _context = context;
        _userProvider = userProvider;
        _userService = userService;
    }

    /// <summary>
    /// Get a user by id
    /// </summary>
    /// <param name="id">The id of the user to get</param>
    /// <returns>The requested user</returns>
    // GET: api/Users/5
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser(string? id)
    {
        if (_context.Users is null)
            return NotFound();

        UserDto? user;

        await Console.Out.WriteLineAsync(_userProvider.GetUserInfo()!.Id);

        try
        {
            user = await _userService.GetUserById(id ?? _userProvider.GetUserInfo()!.Id);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Update a user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A result indicating success or failure</returns>
    // PUT: api/Users/5
    [HttpPut]
    public async Task<ActionResult<UserDto>> PutUser(UpdateUserRequest request)
    {
        var id = _userProvider.GetUserInfo()!.Id;
        UserDto? user;

        try
        {
            user = await _userService.EditUser(id, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(user);
    }

    /// <summary>
    /// Change a user's password
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A result indicating success or failure</returns>
    // PATCH: api/Users/5
    [HttpPatch]
    public async Task<ActionResult<UserDto>> ChangePassword(ChangePasswordUserRequest request)
    {
        var id = _userProvider.GetUserInfo()!.Id;
        UserDto? user;

        try
        {
            user = await _userService.ChangeUserPassword(id, request);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (EntityPasswordInvalidException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("A problem has occurred please try again later");
        }

        return Ok(user);
    }

    /// <summary>
    /// Delete a user by id
    /// </summary>
    /// <returns>A result indicating success or failure</returns>
    // DELETE: api/Users/5
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        var id = _userProvider.GetUserInfo()!.Id;

        try
        {
            var condition = await _userService.DeleteUser(id);

            if (!condition)
                return BadRequest(
                    "There was a problem trying to delete a user, please try again later"
                );
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
}
