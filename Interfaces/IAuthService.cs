using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IAuthService
{
    Task<UserResponseDto?> RegisterAsync(RegisterDto dto);
    Task<LoginResultDto?> LoginAsync(LoginDto dto);
}