namespace ClinicApiLearn.DTOs;

public class PermissionResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}