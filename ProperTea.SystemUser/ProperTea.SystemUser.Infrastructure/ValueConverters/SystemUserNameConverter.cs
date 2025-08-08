using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ProperTea.SystemUser.Domain.ValueObjects;

namespace ProperTea.SystemUser.Infrastructure.ValueConverters;

public class SystemUserNameConverter() : ValueConverter<SystemUserName, string>(
    v => v.Value,
    v => SystemUserName.Create(v));