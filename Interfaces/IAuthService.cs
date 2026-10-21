using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(RegisterDto dto);
    Task<LoginResultDto> LoginAsync(LoginDto dto,string? ipAddress, string? userAgent);// add usersession properties
     Task<List<SessionResponseDto>> GetMySessionsAsync(Guid userId, string? currentRefreshToken);
    Task RevokeSessionAsync(Guid userId, Guid sessionId);
    Task LogoutAllAsync(Guid userId);
     Task<LoginResultDto> RefreshAsync(string refreshToken, string? ipAddress, string? userAgent);
    Task LogoutAsync(string refreshToken); 
}