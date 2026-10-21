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

       public async Task<AppointmentResponseDto> GetByIdAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        return ToDto(appointment);
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto, Guid userId)
    {
        var appointment = new Appointment
        {
            PatientName = dto.PatientName,
            Reason = dto.Reason,
            AppointmentDate = dto.AppointmentDate.ToUniversalTime(),
            CreatedByUserId = userId
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return ToDto(appointment);
    }

    public async Task<AppointmentResponseDto> UpdateAsync(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
         throw new KeyNotFoundException("Appointment not found.");
        }

        appointment.PatientName = dto.PatientName;
        appointment.Reason = dto.Reason;
        appointment.AppointmentDate = dto.AppointmentDate.ToUniversalTime();

        await _context.SaveChangesAsync();

        return ToDto(appointment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
           throw new KeyNotFoundException("Appointment not found.");
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        
    }


    public async Task<AppointmentResponseDto> WritePrescriptionAsync(Guid id, string prescription)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }
        appointment.Prescription = prescription;
        await _context.SaveChangesAsync();

        return ToDto(appointment);
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