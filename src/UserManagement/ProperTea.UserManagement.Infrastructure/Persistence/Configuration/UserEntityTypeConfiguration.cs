using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Infrastructure.Persistence.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
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
    }
}