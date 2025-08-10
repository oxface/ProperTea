using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProperTea.Organization.Infrastructure.Data;

public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Organization>
{
    public void Configure(EntityTypeBuilder<Domain.Organization> builder)
    {
        builder.ToTable("Organizations");
        
        builder.OwnsOne(o => o.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(200)
                .IsRequired();
            name.HasIndex(l => l.Value, "IX_Organization_Name")
                .IsUnique();
        });
    }
}