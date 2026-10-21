namespace ClinicApiLearn.Models;

public class RefreshToken : BaseEntity
{
    public required string TokenHash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}