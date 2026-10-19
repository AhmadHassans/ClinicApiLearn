using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);
        if (user == null)
        {
            return Conflict(new { message = "Email already registered." });
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        SetAuthCookies(result);
        return Ok(result.User);
    }

  
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "No refresh token." });
        }

        var result = await _authService.RefreshAsync(refreshToken);
        if (result == null)
        {
            ClearAuthCookies();
            return Unauthorized(new { message = "Session expired. Please login again." });
        }

        SetAuthCookies(result);
        return Ok(result.User);
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
}