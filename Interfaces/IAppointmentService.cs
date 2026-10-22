using ClinicApiLearn.DTOs;
using ClinicApiLearn.Models;

namespace ClinicApiLearn.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> UpdateStatusAsync(Guid id, AppointmentStatus status);
    Task<PagedResult<AppointmentResponseDto>> GetAllAsync(AppointmentQueryDto query);
    Task<AppointmentResponseDto> GetByIdAsync(Guid id);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto, Guid userId);
    Task<AppointmentResponseDto> UpdateAsync(Guid id, UpdateAppointmentDto dto);
    Task DeleteAsync(Guid id);
    Task<AppointmentResponseDto> WritePrescriptionAsync(Guid id, string prescription);
}