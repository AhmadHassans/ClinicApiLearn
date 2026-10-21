using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionResponseDto>> GetAllAsync()
    {
        return await _context.Permissions
            .OrderBy(p => p.Code)
            .Select(p => new PermissionResponseDto { Id = p.Id, Name = p.Name, Code = p.Code })
            .ToListAsync();
    }
}