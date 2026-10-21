using ClinicApiLearn.DTOs;
namespace ClinicApiLearn.Interfaces;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> UpdateRoleAsync(Guid userId, Guid roleId,Guid currentUserId);
    
}