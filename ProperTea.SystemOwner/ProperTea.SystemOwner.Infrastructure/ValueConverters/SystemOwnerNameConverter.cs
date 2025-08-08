using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ProperTea.SystemOwner.Domain.ValueObjects;

namespace ProperTea.SystemOwner.Infrastructure.ValueConverters;

public class SystemOwnerNameConverter() : ValueConverter<SystemOwnerName, string>(
    v => v.Value,
    v => SystemOwnerName.Create(v));