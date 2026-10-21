using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class PrescriptionDto
{
    [Required]
    [MaxLength(2000)]
    public required string Prescription { get; set; }
}