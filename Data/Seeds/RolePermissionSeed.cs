using ClinicApiLearn.Constants;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data.Seeds;

public static class RolePermissionSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var all = new[]
        {
            PermissionIds.AppointmentRead, PermissionIds.AppointmentCreate, PermissionIds.AppointmentUpdate,
            PermissionIds.AppointmentDelete, PermissionIds.PrescriptionWrite, PermissionIds.UserManage
        };

        var links = new List<RolePermission>();

        // Admin aur SuperAdmin: 
        links.AddRange(all.Select(p => new RolePermission { RoleId = RoleIds.Admin, PermissionId = p }));
        links.AddRange(all.Select(p => new RolePermission { RoleId = RoleIds.SuperAdmin, PermissionId = p }));

        // Doctor: view + write. prescription
        links.Add(new RolePermission { RoleId = RoleIds.Doctor, PermissionId = PermissionIds.AppointmentRead });
        links.Add(new RolePermission { RoleId = RoleIds.Doctor, PermissionId = PermissionIds.PrescriptionWrite });

        // Receptionist: view, create, update
        links.Add(new RolePermission { RoleId = RoleIds.Receptionist, PermissionId = PermissionIds.AppointmentRead });
        links.Add(new RolePermission { RoleId = RoleIds.Receptionist, PermissionId = PermissionIds.AppointmentCreate });
        links.Add(new RolePermission { RoleId = RoleIds.Receptionist, PermissionId = PermissionIds.AppointmentUpdate });

        modelBuilder.Entity<RolePermission>().HasData(links);
    }
}