namespace ClinicApiLearn.Models;

public class Permission : BaseEntity
{
    public required string Name { get; set; }
    public required string Code { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}