using ProperTea.Cqrs;
using ProperTea.UserManagement.Application.Models;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Queries;

public record GetUserByEmailQuery(string Email) : IQuery;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, SystemUserModel?>
{
    private readonly ISystemUserRepository userRepository;

    public GetUserByEmailQueryHandler(ISystemUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<SystemUserModel?> HandleAsync(GetUserByEmailQuery query,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(query.Email, ct);
        return user == null ? null : SystemUserModel.FromEntity(user);
    }
}