using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route(ApiRoutes.Roles)]
[Authorize(Policy = PermissionCodes.UserManage)]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto)
    {
        var role = await _roleService.CreateAsync(dto);
        return Created($"/{ApiRoutes.Roles}/{role.Id}", role);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleDto dto)
    {
        var role = await _roleService.UpdateAsync(id, dto);
        return Ok(role);
    }
}