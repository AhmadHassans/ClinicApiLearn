using System.ComponentModel.DataAnnotations;
using ClinicApiLearn.Models;

namespace ClinicApiLearn.DTOs;

public class UpdateAppointmentStatusDto
{
    [Required]
    [EnumDataType(typeof(AppointmentStatus))]
    public required AppointmentStatus Status { get; set; }
}