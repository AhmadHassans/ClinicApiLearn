using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class CreateRoleDto
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    public required List<Guid> PermissionIds { get; set; }
}