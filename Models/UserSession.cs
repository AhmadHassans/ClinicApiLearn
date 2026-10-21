namespace ClinicApiLearn.Models;

public class UserSession : BaseEntity
{
    public required string TokenHash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime LastUsedAt { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}