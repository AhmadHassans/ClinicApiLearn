using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IPermissionService
{
    Task<List<PermissionResponseDto>> GetAllAsync();
}