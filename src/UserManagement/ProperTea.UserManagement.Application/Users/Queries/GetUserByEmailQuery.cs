using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Application.Users.Models;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Queries;

public record GetUserByEmailQuery(string Email) : IQuery;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserModel?>
{
    private readonly IUserRepository userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<UserModel?> HandleAsync(GetUserByEmailQuery query,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(query.Email, ct);
        return user == null ? null : UserModel.FromEntity(user);
    }
}