using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IAppointmentService
{
    Task<PagedResult<AppointmentResponseDto>> GetAllAsync(AppointmentQueryDto query);
    Task<AppointmentResponseDto> GetByIdAsync(Guid id);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto,Guid userId);
    Task<AppointmentResponseDto> UpdateAsync(Guid id, UpdateAppointmentDto dto);
    Task DeleteAsync(Guid id);
    Task<AppointmentResponseDto> WritePrescriptionAsync(Guid id, string prescription);
}