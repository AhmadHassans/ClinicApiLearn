using ClinicApiLearn.Data.Seeds;
using ClinicApiLearn.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data;

public class AppDbContext : DbContext
{
    // connection string Program.cs se options ke zariye aati hai
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ===== TABLES =====
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);





        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });


        modelBuilder.Entity<Appointment>()
     .HasIndex(a => a.Status);


        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();


        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Code)
            .IsUnique();



        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();


        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();


        modelBuilder.Entity<UserSession>()
            .HasIndex(t => t.TokenHash)
            .IsUnique();


        modelBuilder.Entity<Appointment>()
            .HasIndex(a => a.AppointmentDate);

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


        RoleSeed.Seed(modelBuilder);
        PermissionSeed.Seed(modelBuilder);
        RolePermissionSeed.Seed(modelBuilder);
    }

    // ===== AUDIT FIELDS =====
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