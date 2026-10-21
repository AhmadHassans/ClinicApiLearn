using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route(ApiRoutes.Users)]
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

    [HttpPut("{id:guid}/role")]
   
    public async Task<IActionResult> UpdateRole(Guid id, UpdateUserRoleDto dto)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.UpdateRoleAsync(id, dto.RoleId, currentUserId);
       

        return Ok(user);
    }
}