using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentResponseDto>> GetAllAsync();
    Task<AppointmentResponseDto?> GetByIdAsync(int id);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
}