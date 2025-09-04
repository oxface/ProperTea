using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Queries;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, SystemUserModel?>
{
    private readonly ISystemUserRepository _userRepository;

    public GetUserByEmailQueryHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<SystemUserModel?> HandleAsync(GetUserByEmailQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(query.Email, cancellationToken);
        return user == null ? null : SystemUserModel.FromEntity(user);
    }
}