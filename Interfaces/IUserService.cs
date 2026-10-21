using ClinicApiLearn.DTOs;
namespace ClinicApiLearn.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> UpdateRoleAsync(int userId, int roleId, int currentUserId);
    
}