using ProperTea.Cqrs;
using ProperTea.UserManagement.Application.Models;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, SystemUserModel?>
{
    private readonly ISystemUserRepository userRepository;

    public GetUserByIdQueryHandler(ISystemUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<SystemUserModel?> HandleAsync(GetUserByIdQuery query,
        CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(query.UserId, ct);
        return user == null ? null : SystemUserModel.FromEntity(user);
    }
}