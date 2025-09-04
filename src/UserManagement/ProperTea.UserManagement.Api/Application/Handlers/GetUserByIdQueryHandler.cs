using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Queries;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, SystemUserModel?>
{
    private readonly ISystemUserRepository _userRepository;

    public GetUserByIdQueryHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<SystemUserModel?> HandleAsync(GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);
        return user == null ? null : SystemUserModel.FromEntity(user);
    }
}