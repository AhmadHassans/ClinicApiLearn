using ClinicApiLearn.Constants;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data.Seeds;

public static class SuperAdminSeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SuperAdminSeed");

        var email = config["SuperAdmin:Email"];
        var password = config["SuperAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("SuperAdmin:Email or SuperAdmin:Password is not configured. Skipping.");
            return;
        }

        // pehle se super admin hai to kuch mat karo
        if (await context.Users.AnyAsync(u => u.RoleId == RoleIds.SuperAdmin))
        {
            return;
        }

        email = email.Trim().ToLowerInvariant();

        if (await context.Users.AnyAsync(u => u.Email == email))
        {
            logger.LogWarning("A user with {Email} already exists. Skipping super admin seed.", email);
            return;
        }

        context.Users.Add(new User
        {
            Username = config["SuperAdmin:Username"] ?? "superadmin",
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            RoleId = RoleIds.SuperAdmin
        });

        await context.SaveChangesAsync();
        logger.LogInformation("Super admin {Email} created.", email);
    }
}