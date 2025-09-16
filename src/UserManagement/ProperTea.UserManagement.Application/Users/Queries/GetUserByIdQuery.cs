using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Application.Users.Models;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery;

public class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserModel?>
{
    public async Task<UserModel?> HandleAsync(GetUserByIdQuery query,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(query.UserId, ct);
        return user == null ? null : UserModel.FromEntity(user);
    }
}