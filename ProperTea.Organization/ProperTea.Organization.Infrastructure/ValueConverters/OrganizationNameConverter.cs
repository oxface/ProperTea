using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ProperTea.Organization.Domain.ValueObjects;

namespace ProperTea.Organization.Infrastructure.ValueConverters;

public class OrganizationNameConverter() : ValueConverter<OrganizationName, string>(
    v => v.Value,
    v => OrganizationName.Create(v));