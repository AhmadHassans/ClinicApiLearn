using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data.Seeds;

public static class PermissionSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var date = SeedConstants.SeedDate;

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "View Appointments", Code = "Appointment.Read", CreatedAt = date },
            new Permission { Id = 2, Name = "Create Appointment", Code = "Appointment.Create", CreatedAt = date },
            new Permission { Id = 3, Name = "Update Appointment", Code = "Appointment.Update", CreatedAt = date },
            new Permission { Id = 4, Name = "Delete Appointment", Code = "Appointment.Delete", CreatedAt = date },
            new Permission { Id = 5, Name = "Write Prescription", Code = "Prescription.Write", CreatedAt = date },
            new Permission { Id = 6, Name = "Manage Users", Code = "User.Manage", CreatedAt = date }
        );
    }
}