using ClinicApiLearn.DTOs;

namespace ClinicApiLearn.Interfaces;

public interface IAppointmentService
{
    Task<List<AppointmentResponseDto>> GetAllAsync();
    Task<AppointmentResponseDto> GetByIdAsync(int id);
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto,int userId);
    Task<AppointmentResponseDto> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task DeleteAsync(int id);
    Task<AppointmentResponseDto> WritePrescriptionAsync(int id, string prescription);
}