using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class UpdateUserRoleDto
{
    [Required]
    public Guid RoleId { get; set; }
}