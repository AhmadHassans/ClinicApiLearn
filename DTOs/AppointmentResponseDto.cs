namespace ClinicApiLearn.DTOs;

public class AppointmentResponseDto
{
    public int Id { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string? Prescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}