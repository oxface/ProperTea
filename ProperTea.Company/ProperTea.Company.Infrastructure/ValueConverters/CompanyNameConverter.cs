using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ProperTea.Company.Domain.ValueObjects;

namespace ProperTea.Company.Infrastructure.ValueConverters;

public class CompanyNameConverter() : ValueConverter<CompanyName, string>(
    v => v.Value,
    v => CompanyName.Create(v));