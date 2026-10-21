namespace ClinicApiLearn.DTOs;

public class LoginResultDto
{
   public required UserResponseDto User { get; set; }
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}