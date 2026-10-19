using ClinicApiLearn.Models;

using Microsoft.EntityFrameworkCore;

namespace ClinicApiLearn.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }// connecting string add from program.cs in :base(optioons)
    public DbSet<Appointment> Appointments => Set<Appointment>();// table  APPOINTMENT 
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
