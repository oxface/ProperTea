using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProperTea.SystemUser.Infrastructure.ValueConverters;

namespace ProperTea.SystemUser.Infrastructure.Data;

public class SystemUserEntityTypeConfiguration : IEntityTypeConfiguration<Domain.SystemUser>
{
    public void Configure(EntityTypeBuilder<Domain.SystemUser> builder)
    {
        builder.Property(c => c.Name)
            .HasColumnType("nvarchar")
            .HasConversion(new SystemUserNameConverter())
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(so => so.Name, @"IX_SystemUser_Name")
            .IsUnique();
    }
}