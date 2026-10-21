using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;
using ClinicApiLearn.Constants;
namespace ClinicApiLearn.Services;

public class AuthService : IAuthService
{
   private static readonly Guid DefaultRoleId = RoleIds.Receptionist;

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

        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
           throw new InvalidOperationException("Email already registered.");
        }

        var role = await _context.Roles.FindAsync(DefaultRoleId);

        var user = new User
        {
            Username = dto.Username.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role!
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return ToDto(user);
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await UsersWithPermissions()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
          throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return await IssueTokensAsync(user);
    }

  
    public async Task<LoginResultDto> RefreshAsync(string refreshToken)
    {
        var hash = _tokenService.HashToken(refreshToken);
        var stored = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash);

        // expired
        if (stored == null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Session expired. Please login again.");
        }

   
        stored.RevokedAt = DateTime.UtcNow;

        var user = await UsersWithPermissions().FirstAsync(u => u.Id == stored.UserId);
        return await IssueTokensAsync(user);
    }

    
    public async Task LogoutAsync(string refreshToken)
    {
        var hash = _tokenService.HashToken(refreshToken);
        var stored = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash && t.RevokedAt == null);

        if (stored != null)
        {
            stored.RevokedAt = DateTime.UtcNow;
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

    
    private async Task<LoginResultDto> IssueTokensAsync(User user)
    {
        var refreshToken = _tokenService.CreateRefreshToken();
        var days = _config.GetValue<int>("Jwt:RefreshTokenDays");

        _context.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(days),
            UserId = user.Id
        });
        await _context.SaveChangesAsync();

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