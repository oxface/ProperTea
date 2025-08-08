using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProperTea.Organization.Infrastructure.ValueConverters;

namespace ProperTea.Organization.Infrastructure.Data;

public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Organization>
{
    public void Configure(EntityTypeBuilder<Domain.Organization> builder)
    {
        builder.Property(c => c.Name)
            .HasColumnType("nvarchar")
            .HasConversion(new OrganizationNameConverter())
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(so => so.Name, @"IX_Name")
            .IsUnique();
    }
}