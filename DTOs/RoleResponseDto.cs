namespace ClinicApiLearn.DTOs;

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<string> Permissions { get; set; }
}