namespace ClinicApiLearn.DTOs;

public class LoginResultDto
{
    public UserResponseDto User { get; set; } = null!;
    public string AccessToken { get; set; } = string.Empty;
}