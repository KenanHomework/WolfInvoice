using WolfInvoice.Interfaces.IProviders;

namespace WolfInvoice.Providers.RequestUser;

/// <summary>
/// Provider for retrieving information about the current user making the request.
/// </summary>
public class RequestUserProvider : IRequestUserProvider
{
    private readonly HttpContext _httpContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpContext"></param>
    public RequestUserProvider(IHttpContextAccessor httpContext) =>
        _httpContext = httpContext.HttpContext!;

    /// <inheritdoc/>
    public UserInfo? GetUserInfo()
    {
        if (!_httpContext.User.Claims.Any())
            return null;

        var userId = _httpContext.User.Claims.First(x => x.Type == "user_id").Value;

        var userName = _httpContext.User.Claims.First(x => x.Type == "user_email").Value;

        return new UserInfo(userId, userName);
    }
}
