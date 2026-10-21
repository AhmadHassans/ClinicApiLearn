namespace ClinicApiLearn.Models;

public class Role : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}