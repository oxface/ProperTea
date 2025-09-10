using ProperTea.Domain.Shared;

namespace ProperTea.UserManagement.Domain.SystemUsers;

public record UserIdentity(Guid Id) : ValueObject
{
}