using ClinicApiLearn.Constants;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data.Seeds;

public static class PermissionSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var date = SeedConstants.SeedDate;

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = PermissionIds.AppointmentRead, Name = "View Appointments", Code = PermissionCodes.AppointmentRead, CreatedAt = date },
            new Permission { Id = PermissionIds.AppointmentCreate, Name = "Create Appointment", Code = PermissionCodes.AppointmentCreate, CreatedAt = date },
            new Permission { Id = PermissionIds.AppointmentUpdate, Name = "Update Appointment", Code = PermissionCodes.AppointmentUpdate, CreatedAt = date },
            new Permission { Id = PermissionIds.AppointmentDelete, Name = "Delete Appointment", Code = PermissionCodes.AppointmentDelete, CreatedAt = date },
            new Permission { Id = PermissionIds.PrescriptionWrite, Name = "Write Prescription", Code = PermissionCodes.PrescriptionWrite, CreatedAt = date },
            new Permission { Id = PermissionIds.UserManage, Name = "Manage Users", Code = PermissionCodes.UserManage, CreatedAt = date }
        );
    }
}