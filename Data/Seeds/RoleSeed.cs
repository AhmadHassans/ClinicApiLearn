using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data.Seeds;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = SeedConstants.SeedDate },
            new Role { Id = 2, Name = "Doctor", CreatedAt = SeedConstants.SeedDate },
            new Role { Id = 3, Name = "Receptionist", CreatedAt = SeedConstants.SeedDate },
            new Role { Id = 4, Name = "SuperAdmin",CreatedAt = SeedConstants.SeedDate}
        );
    }
}