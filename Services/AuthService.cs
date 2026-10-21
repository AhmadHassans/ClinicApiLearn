using ClinicApiLearn.Constants;
using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, ITokenService tokenService, IConfiguration config)
    {
        _context = context;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var username = dto.Username.Trim();

        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            throw new InvalidOperationException("Email already registered.");
        }

        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            throw new InvalidOperationException("Username already taken.");
        }

        // super admin sirf seed se banta hai, koi register se nahi bana sakta
        if (dto.RoleId == RoleIds.SuperAdmin)
        {
            throw new InvalidOperationException("Super admin role cannot be assigned.");
        }

        var role = await _context.Roles.FindAsync(dto.RoleId);
        if (role == null)
        {
            throw new KeyNotFoundException("Role not found.");
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ToDto(user);
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto, string? ipAddress, string? userAgent)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await UsersWithPermissions()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // har login = ek naya session
        var refreshToken = _tokenService.CreateRefreshToken();
        var now = DateTime.UtcNow;

        _context.UserSessions.Add(new UserSession
        {
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = now.AddDays(RefreshTokenDays()),
            LastUsedAt = now,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            UserId = user.Id
        });
        await _context.SaveChangesAsync();

        return BuildResult(user, refreshToken);
    }

    public async Task<LoginResultDto> RefreshAsync(string refreshToken, string? ipAddress, string? userAgent)
    {
        var hash = _tokenService.HashToken(refreshToken);
        var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.TokenHash == hash);

        // na mila, logout ho chuka, ya expire
        if (session == null || session.RevokedAt != null || session.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Session expired. Please login again.");
        }

        // rotation: wahi session, naya token (purana token ab kaam nahi karega)
        var newRefreshToken = _tokenService.CreateRefreshToken();
        var now = DateTime.UtcNow;

        session.TokenHash = _tokenService.HashToken(newRefreshToken);
        session.ExpiresAt = now.AddDays(RefreshTokenDays());
        session.LastUsedAt = now;
        session.IpAddress = ipAddress;
        session.UserAgent = userAgent;
        await _context.SaveChangesAsync();

        var user = await UsersWithPermissions().FirstAsync(u => u.Id == session.UserId);
        return BuildResult(user, newRefreshToken);
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var hash = _tokenService.HashToken(refreshToken);
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.TokenHash == hash && s.RevokedAt == null);

        if (session != null)
        {
            session.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    private IQueryable<User> UsersWithPermissions()
    {
        return _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
    }

    private int RefreshTokenDays()
    {
        return _config.GetValue<int>("Jwt:RefreshTokenDays");
    }

    private LoginResultDto BuildResult(User user, string refreshToken)
    {
        return new LoginResultDto
        {
            User = ToDto(user),
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = refreshToken
        };
    }

    private static UserResponseDto ToDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.Name
        };
    }
}