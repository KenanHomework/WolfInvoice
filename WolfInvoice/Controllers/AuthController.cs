using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.DTOs;
using WolfInvoice.DTOs.Requests.Auth;
using WolfInvoice.Interfaces.EntityServices;
using WolfInvoice.Interfaces.Services;
using WolfInvoice.Models.DataModels;
using WolfInvoice.Services;

namespace WolfInvoice.Controllers;

/// <summary>
/// Controller for handling user authentication
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly WolfInvoiceContext _context;
    private readonly CryptService _cryptService;
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="jwtService">The JWT service instance used for generating access tokens.</param>
    /// <param name="context">The Database Context used for access DB</param>
    /// <param name="userService"></param>
    /// <param name="cryptService"></param>
    public AuthController(
        IJwtService jwtService,
        WolfInvoiceContext context,
        IUserService userService,
        CryptService cryptService
    )
    {
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _cryptService = cryptService;
    }

    /// <summary>
    /// Endpoint for registering a new user
    /// </summary>
    /// <param name="request">The registration request containing user email and password</param>
    /// <returns>The generated access token for the new user</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthTokenDto>> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower().Equals(request.Email.ToLower())
        );

        if (existingUser is not null)
            return Conflict("User already exists");

        var user = new User()
        {
            Id = IdGeneratorService.GetUniqueId(),
            Name = request.Name,
            Password = _cryptService.CryptPassword(request.Password),
            Email = request.Email.ToLower()
        };

        await _userService.AddUser(user);

        var accessToken = _jwtService.GenerateSecurityToken(user.Id, request.Email);
        return new AuthTokenDto { AccessToken = accessToken };
    }

    /// <summary>
    /// Endpoint for user login.
    /// </summary>
    /// <param name="request">The login request containing user email and password</param>
    /// <returns>The generated access token for the logged in user</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthTokenDto>> Login([FromBody] LoginRequest request)
    {
        // Get user
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower().Equals(request.Email.ToLower())
        );

        if (user is null)
            return NotFound();

        if (!_cryptService.CheckPassword(request.Password, user.Password))
            return Unauthorized();

        var accessToken = _jwtService.GenerateSecurityToken(user.Id, request.Email);
        return new AuthTokenDto { AccessToken = accessToken };
    }
}
