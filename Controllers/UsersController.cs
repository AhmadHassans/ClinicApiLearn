using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionCodes.UserManage)]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpPut("{id}/role")]
   
    public async Task<IActionResult> UpdateRole(int id, UpdateUserRoleDto dto)
    {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.UpdateRoleAsync(id, dto.RoleId, currentUserId);
        if (user == null)
        {
            return NotFound(new { message = "User or role not found." });
        }

        return Ok(user);
    }
}