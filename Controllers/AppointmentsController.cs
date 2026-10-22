using System.Security.Claims;
using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route(ApiRoutes.Appointments)]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }


    [HttpPut("{id:guid}/status")]
    [Authorize(Policy = PermissionCodes.AppointmentUpdate)]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateAppointmentStatusDto dto)
    {
        var appointment = await _appointmentService.UpdateStatusAsync(id, dto.Status);
        return Ok(appointment);
    }


    [HttpGet]
    [Authorize(Policy = PermissionCodes.AppointmentRead)]
    public async Task<IActionResult> GetAll([FromQuery] AppointmentQueryDto query)
    {
        var appointments = await _appointmentService.GetAllAsync(query);
        return Ok(appointments);
    }


    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermissionCodes.AppointmentRead)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        return Ok(appointment);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.AppointmentCreate)]
    public async Task<IActionResult> Create(CreateAppointmentDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var appointment = await _appointmentService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PermissionCodes.AppointmentUpdate)]
    public async Task<IActionResult> Update(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentService.UpdateAsync(id, dto);


        return Ok(appointment);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PermissionCodes.AppointmentDelete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _appointmentService.DeleteAsync(id);


        return NoContent();
    }

    [HttpPut("{id:guid}/prescription")]
    [Authorize(Policy = PermissionCodes.PrescriptionWrite)]
    public async Task<IActionResult> WritePrescription(Guid id, PrescriptionDto dto)
    {
        var appointment = await _appointmentService.WritePrescriptionAsync(id, dto.Prescription);


        return Ok(appointment);
    }
}