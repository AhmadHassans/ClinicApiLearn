using ClinicApiLearn.Models;

using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }// connecting string add from program.cs in :base(optioons)
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();                              
    public DbSet<Role> Roles => Set<Role>();                               
    public DbSet<Permission> Permissions => Set<Permission>();             
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>(); 
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

   // understanding logic for ef core
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

       
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();

                
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.CreatedByUser)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // =====  SEEDS =====
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = seedDate },
            new Role { Id = 2, Name = "Doctor", CreatedAt = seedDate },
            new Role { Id = 3, Name = "Receptionist", CreatedAt = seedDate }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "View Appointments", Code = "Appointment.Read", CreatedAt = seedDate },
            new Permission { Id = 2, Name = "Create Appointment", Code = "Appointment.Create", CreatedAt = seedDate },
            new Permission { Id = 3, Name = "Update Appointment", Code = "Appointment.Update", CreatedAt = seedDate },
            new Permission { Id = 4, Name = "Delete Appointment", Code = "Appointment.Delete", CreatedAt = seedDate },
            new Permission { Id = 5, Name = "Write Prescription", Code = "Prescription.Write", CreatedAt = seedDate },
            new Permission { Id = 6, Name = "Manage Users", Code = "User.Manage", CreatedAt = seedDate }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            // Admin: all permission 
            new RolePermission { RoleId = 1, PermissionId = 1 },
            new RolePermission { RoleId = 1, PermissionId = 2 },
            new RolePermission { RoleId = 1, PermissionId = 3 },
            new RolePermission { RoleId = 1, PermissionId = 4 },
            new RolePermission { RoleId = 1, PermissionId = 5 },
            new RolePermission { RoleId = 1, PermissionId = 6 },
            // Doctor: view + prescription
            new RolePermission { RoleId = 2, PermissionId = 1 },
            new RolePermission { RoleId = 2, PermissionId = 5 },
            // Receptionist: view, create, update
            new RolePermission { RoleId = 3, PermissionId = 1 },
            new RolePermission { RoleId = 3, PermissionId = 2 },
            new RolePermission { RoleId = 3, PermissionId = 3 }
        );
    }
       public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Property(e => e.CreatedAt).IsModified = false;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


}
