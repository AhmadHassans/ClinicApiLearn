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

    // ===== NAYA: filter + search + sort + pagination =====
    public async Task<PagedResult<AppointmentResponseDto>> GetAllAsync(AppointmentQueryDto query)
    {
    
        IQueryable<Appointment> appointments = _context.Appointments;

        // 1. FILTER
        if (query.FromDate != null)
        {
            var from = query.FromDate.Value.ToUniversalTime();
            appointments = appointments.Where(a => a.AppointmentDate >= from);
        }

        if (query.ToDate != null)
        {
            var to = query.ToDate.Value.ToUniversalTime();
            appointments = appointments.Where(a => a.AppointmentDate <= to);
        }

        if (query.HasPrescription != null)
        {
            appointments = query.HasPrescription.Value
                ? appointments.Where(a => a.Prescription != null)
                : appointments.Where(a => a.Prescription == null);
        }
        // STATUS
           if (query.Status != null)
        {
            appointments = appointments.Where(a => a.Status == query.Status);
        }

        // 2. SEARCH 
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            appointments = appointments.Where(a =>
                EF.Functions.ILike(a.PatientName, pattern) ||
                EF.Functions.ILike(a.Reason, pattern));
        }

        // 3. COUNT 
        var totalCount = await appointments.CountAsync();

        // 4. SORT
        var descending = query.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

        appointments = query.SortBy.ToLowerInvariant() switch
        {
            "date" => descending
                ? appointments.OrderByDescending(a => a.AppointmentDate)
                : appointments.OrderBy(a => a.AppointmentDate),
            "patientname" => descending
                ? appointments.OrderByDescending(a => a.PatientName)
                : appointments.OrderBy(a => a.PatientName),
            _ => descending
                ? appointments.OrderByDescending(a => a.CreatedAt)
                : appointments.OrderBy(a => a.CreatedAt)
        };

        // 5. PAGINATE 
        var items = await appointments
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(a => new AppointmentResponseDto
            {
                Id = a.Id,
                PatientName = a.PatientName,
                Reason = a.Reason,
                AppointmentDate = a.AppointmentDate,
                Prescription = a.Prescription,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToListAsync();

        return new PagedResult<AppointmentResponseDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
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

        // patient status cancel 
        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot write prescription for a cancelled appointment.");
        }

        appointment.Prescription = prescription;

        // patient status complete
        appointment.Status = AppointmentStatus.Completed;

        await _context.SaveChangesAsync();

        return ToDto(appointment);
    }

    public async Task<AppointmentResponseDto> UpdateStatusAsync(Guid id, AppointmentStatus status)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

       
        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new InvalidOperationException("Cancelled appointment status cannot be changed.");
        }

        appointment.Status = status;
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
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}