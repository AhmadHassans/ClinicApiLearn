using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class UpdateAppointmentDto
{
    [Required]
    [MaxLength(100)]
    public required string PatientName { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Reason { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }
}