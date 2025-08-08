using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProperTea.Company.Infrastructure.Company.ValueConverters;

namespace ProperTea.Company.Infrastructure.Company.Data;

public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Company.Company>
{
    public void Configure(EntityTypeBuilder<Domain.Company.Company> builder)
    {
        builder.Property(c => c.Name)
            .HasConversion(new CompanyNameConverter())
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(nameof(Domain.Company.Company.Name),
                nameof(Domain.Company.Company.SystemOwnerId))
            .IsUnique()
            .HasDatabaseName("IX_Company_Name");

        builder.HasIndex(c => c.SystemOwnerId, "IX_Company_SystemOwnerId");
    }
}