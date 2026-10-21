using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IRoleService
{
    Task<List<RoleResponseDto>> GetAllAsync();
    Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
    Task<RoleResponseDto> UpdateAsync(Guid id, UpdateRoleDto dto);
}