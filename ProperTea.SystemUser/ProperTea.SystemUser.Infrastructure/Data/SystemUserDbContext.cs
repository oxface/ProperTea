using Microsoft.EntityFrameworkCore;

namespace ProperTea.SystemUser.Infrastructure.Data;

public class SystemUserDbContext(DbContextOptions<SystemUserDbContext> options) : DbContext(options)
{
    public DbSet<Domain.SystemUser> SystemUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SystemUserEntityTypeConfiguration());
    }
}