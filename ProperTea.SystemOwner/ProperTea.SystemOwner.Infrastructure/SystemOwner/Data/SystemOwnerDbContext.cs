using Microsoft.EntityFrameworkCore;

namespace ProperTea.SystemOwner.Infrastructure.SystemOwner.Data;

public class SystemOwnerDbContext(DbContextOptions<SystemOwnerDbContext> options) : DbContext(options)
{
    public DbSet<Domain.SystemOwner.SystemOwner> SystemOwners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SystemOwnerEntityTypeConfiguration());
    }
}