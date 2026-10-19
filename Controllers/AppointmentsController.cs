using System.Security.Claims;
using ClinicApiLearn.Constants;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApiLearn.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    [Authorize(Policy = PermissionCodes.AppointmentRead)]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(appointments);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PermissionCodes.AppointmentRead)]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpPost]
    [Authorize(Policy = PermissionCodes.AppointmentCreate)]
    public async Task<IActionResult> Create(CreateAppointmentDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var appointment = await _appointmentService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionCodes.AppointmentUpdate)]
    public async Task<IActionResult> Update(int id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentService.UpdateAsync(id, dto);
        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionCodes.AppointmentDelete)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _appointmentService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{id}/prescription")]
    [Authorize(Policy = PermissionCodes.PrescriptionWrite)]
    public async Task<IActionResult> WritePrescription(int id, PrescriptionDto dto)
    {
        var appointment = await _appointmentService.WritePrescriptionAsync(id, dto.Prescription);
        if (appointment == null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }
}