using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ProperTea.SystemOwner.Domain.SystemOwner.ValueObjects;

namespace ProperTea.SystemOwner.Infrastructure.SystemOwner.ValueConverters;

public class SystemOwnerNameConverter() : ValueConverter<SystemOwnerName, string>(
    v => v.Value,
    v => SystemOwnerName.Create(v));