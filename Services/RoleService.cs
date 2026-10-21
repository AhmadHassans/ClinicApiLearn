using ClinicApiLearn.Constants;
using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Services;

public class RoleService : IRoleService
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleResponseDto>> GetAllAsync()
    {
        return await _context.Roles
            .OrderBy(r => r.Name)
            .Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                Permissions = r.RolePermissions.Select(rp => rp.Permission.Code).ToList()
            })
            .ToListAsync();
    }

    public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
    {
        var name = dto.Name.Trim();

        if (await _context.Roles.AnyAsync(r => r.Name == name))
        {
            throw new InvalidOperationException("Role name already exists.");
        }

        var permissions = await GetPermissionsOrThrowAsync(dto.PermissionIds);

        var role = new Role { Name = name };
        foreach (var permission in permissions)
        {
            role.RolePermissions.Add(new RolePermission { PermissionId = permission.Id });
        }

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return ToDto(role, permissions);
    }

    public async Task<RoleResponseDto> UpdateAsync(Guid id, UpdateRoleDto dto)
    {
 
        if (id == RoleIds.SuperAdmin)
        {
            throw new InvalidOperationException("Super admin role cannot be changed.");
        }

        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
        {
            throw new KeyNotFoundException("Role not found.");
        }

        var name = dto.Name.Trim();
        if (await _context.Roles.AnyAsync(r => r.Name == name && r.Id != id))
        {
            throw new InvalidOperationException("Role name already exists.");
        }

        var permissions = await GetPermissionsOrThrowAsync(dto.PermissionIds);
        var newIds = permissions.Select(p => p.Id).ToHashSet();
        var oldIds = role.RolePermissions.Select(rp => rp.PermissionId).ToHashSet();

        var toRemove = role.RolePermissions.Where(rp => !newIds.Contains(rp.PermissionId)).ToList();
        _context.RolePermissions.RemoveRange(toRemove);

    
        foreach (var permissionId in newIds.Where(pid => !oldIds.Contains(pid)))
        {
            _context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
        }

        role.Name = name;
        await _context.SaveChangesAsync();

        return ToDto(role, permissions);
    }

    // all ids are correct 
    private async Task<List<Permission>> GetPermissionsOrThrowAsync(List<Guid> permissionIds)
    {
        var ids = permissionIds.Distinct().ToList();
        var permissions = await _context.Permissions.Where(p => ids.Contains(p.Id)).ToListAsync();

        if (permissions.Count != ids.Count)
        {
            throw new KeyNotFoundException("One or more permissions not found.");
        }

        return permissions;
    }

    private static RoleResponseDto ToDto(Role role, List<Permission> permissions)
    {
        return new RoleResponseDto
        {
            Id = role.Id,
            Name = role.Name,
            Permissions = permissions.Select(p => p.Code).OrderBy(c => c).ToList()
        };
    }
}