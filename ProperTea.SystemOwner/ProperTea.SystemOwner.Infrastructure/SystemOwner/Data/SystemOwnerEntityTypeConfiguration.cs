using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProperTea.SystemOwner.Infrastructure.SystemOwner.ValueConverters;

namespace ProperTea.SystemOwner.Infrastructure.SystemOwner.Data;

public class SystemOwnerEntityTypeConfiguration : IEntityTypeConfiguration<Domain.SystemOwner.SystemOwner>
{
    public void Configure(EntityTypeBuilder<Domain.SystemOwner.SystemOwner> builder)
    {
        builder.Property(c => c.Name)
            .HasColumnType("nvarchar")
            .HasConversion(new SystemOwnerNameConverter())
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(so => so.Name, @"IX_SystemOwner_Name")
            .IsUnique();
    }
}