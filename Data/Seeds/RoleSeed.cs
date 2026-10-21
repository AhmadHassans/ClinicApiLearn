using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;
using ClinicApiLearn.Constants;

namespace ClinicApiLearn.Data.Seeds;

public static class RoleSeed
{
    public static void Seed(ModelBuilder modelBuilder)

    {
         var date = SeedConstants.SeedDate;
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = RoleIds.Admin, Name = "Admin", CreatedAt = date },
            new Role { Id = RoleIds.Doctor, Name = "Doctor", CreatedAt = date },
            new Role { Id = RoleIds.Receptionist, Name = "Receptionist", CreatedAt = date },
            new Role { Id = RoleIds.SuperAdmin, Name = "SuperAdmin", CreatedAt = date }
        );
    }
}