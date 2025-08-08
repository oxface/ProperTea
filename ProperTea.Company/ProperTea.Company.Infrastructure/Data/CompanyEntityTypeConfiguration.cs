using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProperTea.Company.Infrastructure.ValueConverters;

namespace ProperTea.Company.Infrastructure.Data;

public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Company>
{
    public void Configure(EntityTypeBuilder<Domain.Company> builder)
    {
        builder.Property(c => c.Name)
            .HasConversion(new CompanyNameConverter())
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(nameof(Domain.Company.Name),
                nameof(Domain.Company.SystemOwnerId))
            .IsUnique()
            .HasDatabaseName("IX_Company_Name");

        builder.HasIndex(c => c.SystemOwnerId, "IX_Company_SystemOwnerId");
    }
}