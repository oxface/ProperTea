using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Infrastructure.Persistence.Configuration;

public class SystemUserEntityTypeConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.OwnsOne(o => o.Email, email =>
        {
            email.Property(n => n.Value)
                .HasMaxLength(200)
                .IsRequired();
            email.HasIndex(l => l.Value, "IX_Organization_Name")
                .IsUnique();
        });

        builder.OwnsOne(o => o.FullName, email =>
        {
            email.Property(n => n.Value)
                .HasMaxLength(200)
                .IsRequired();
            email.HasIndex(l => l.Value, "IX_Organization_Name")
                .IsUnique();
        });

        builder.OwnsMany(o => o.UserIdentities);
        builder.OwnsMany(o => o.OrganizationMemberships);
    }
}