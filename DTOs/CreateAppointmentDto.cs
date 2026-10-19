using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class CreateAppointmentDto
{
    [Required]
    [MaxLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public DateTime AppointmentDate { get; set; }
}