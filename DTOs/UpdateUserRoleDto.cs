using System.ComponentModel.DataAnnotations;

namespace ClinicApiLearn.DTOs;

public class UpdateUserRoleDto
{
    [Required]
    public int RoleId { get; set; }
}