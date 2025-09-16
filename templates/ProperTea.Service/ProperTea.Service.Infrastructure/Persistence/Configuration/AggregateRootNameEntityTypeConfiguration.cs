using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProperTea.Service.Domain.AggregateRootNames;

namespace ProperTea.Service.Infrastructure.Persistence.Configuration;

public class AggregateRootNameEntityTypeConfiguration : IEntityTypeConfiguration<AggregateRootName>
{
    public void Configure(EntityTypeBuilder<AggregateRootName> builder)
    {
        // TODO: implement entity configuration
    }
}