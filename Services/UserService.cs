using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    
    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .OrderBy(u => u.Id)
            .ToListAsync();

        return users.Select(ToDto).ToList();
    }

    public async Task<UserResponseDto?> UpdateRoleAsync(int userId, int roleId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var role = await _context.Roles.FindAsync(roleId);

        if (user == null || role == null)
        {
            return null;
        }

        user.Role = role;
        await _context.SaveChangesAsync();

        return ToDto(user);
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