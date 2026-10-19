using ClinicApiLearn.Data;
using ClinicApiLearn.DTOs;
using ClinicApiLearn.Interfaces;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentResponseDto>> GetAllAsync()
    {
        var appointments = await _context.Appointments
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return appointments.Select(ToDto).ToList();
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);

        return appointment == null ? null : ToDto(appointment);
    }
    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            PatientName = dto.PatientName,
            Reason = dto.Reason,
            AppointmentDate = dto.AppointmentDate.ToUniversalTime()
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return ToDto(appointment);
    }

    public async Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return null;
        }

        appointment.PatientName = dto.PatientName;
        appointment.Reason = dto.Reason;
        appointment.AppointmentDate = dto.AppointmentDate.ToUniversalTime();

        await _context.SaveChangesAsync();

        return ToDto(appointment);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return true;
    }

    private static AppointmentResponseDto ToDto(Appointment a)
    {
        return new AppointmentResponseDto
        {
            Id = a.Id,
            PatientName = a.PatientName,
            Reason = a.Reason,
            AppointmentDate = a.AppointmentDate,
            Prescription = a.Prescription,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}