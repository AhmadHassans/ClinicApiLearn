using ClinicApiLearn.Constants;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route(ApiRoutes.Permissions)]
[Authorize(Policy = PermissionCodes.UserManage)]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var permissions = await _permissionService.GetAllAsync();
        return Ok(permissions);
    }
}