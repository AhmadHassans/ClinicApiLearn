using System.Security.Claims;
using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route(ApiRoutes.Auth)]
public class AuthController : ControllerBase
{
    private const string AccessTokenCookie = "access_token";
    private const string RefreshTokenCookie = "refresh_token";

    private readonly IAuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(IAuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    [HttpPost("register")]
    [Authorize(Policy = PermissionCodes.UserManage)]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto, ClientIp(), ClientUserAgent());
        SetAuthCookies(result);
        return Ok(result.User);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("No refresh token.");
        }

        try
        {
            var result = await _authService.RefreshAsync(refreshToken, ClientIp(), ClientUserAgent());
            SetAuthCookies(result);
            return Ok(result.User);
        }
        catch (UnauthorizedAccessException)
        {
          
            ClearAuthCookies();
            throw;
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookie];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }

        ClearAuthCookies();
        return NoContent();
    }

  
    [HttpGet("sessions")]
    [Authorize]
    public async Task<IActionResult> GetMySessions()
    {
        var sessions = await _authService.GetMySessionsAsync(CurrentUserId(), Request.Cookies[RefreshTokenCookie]);
        return Ok(sessions);
    }

    
    [HttpDelete("sessions/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> RevokeSession(Guid id)
    {
        await _authService.RevokeSessionAsync(CurrentUserId(), id);
        return NoContent();
    }

    
    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        await _authService.LogoutAllAsync(CurrentUserId());
        ClearAuthCookies();
        return NoContent();
    }

    private void SetAuthCookies(LoginResultDto result)
    {
        var minutes = _config.GetValue<int>("Jwt:AccessTokenMinutes");
        var days = _config.GetValue<int>("Jwt:RefreshTokenDays");

        Response.Cookies.Append(AccessTokenCookie, result.AccessToken,
            BuildCookieOptions(DateTimeOffset.UtcNow.AddMinutes(minutes), "/"));

        Response.Cookies.Append(RefreshTokenCookie, result.RefreshToken,
            BuildCookieOptions(DateTimeOffset.UtcNow.AddDays(days), "/api/auth"));
    }

    private void ClearAuthCookies()
    {
        Response.Cookies.Delete(AccessTokenCookie, BuildCookieOptions(null, "/"));
        Response.Cookies.Delete(RefreshTokenCookie, BuildCookieOptions(null, "/api/auth"));
    }

    private static CookieOptions BuildCookieOptions(DateTimeOffset? expires, string path)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires,
            Path = path
        };
    }

    // card  login  user  Id
    private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // request IP
    private string? ClientIp() => HttpContext.Connection.RemoteIpAddress?.ToString();

    // request  browser / app
    private string? ClientUserAgent() => Request.Headers.UserAgent.ToString();
}