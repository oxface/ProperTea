using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Models;

public record UserModel(
    Guid Id,
    string Email,
    string FullName,
    DateTime CreatedAt,
    bool IsActive
)
{
    public static UserModel FromEntity(User user)
    {
        return new UserModel(
            user.Id,
            user.Email,
            user.FullName,
            user.CreatedAt,
            user.IsActive);
    }
}