namespace ClinicApiLearn.DTOs;

public class AppointmentResponseDto
{
    public int Id { get; set; }
    public required string PatientName { get; set; }
    public required string Reason { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Prescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}