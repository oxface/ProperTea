using ProperTea.Contracts.CQRS;
using ProperTea.UserManagement.Api.Application.Queries;
using ProperTea.UserManagement.Api.Domain.Users;

namespace ProperTea.UserManagement.Api.Application.Handlers;

public class CheckUserExistsQueryHandler : IQueryHandler<CheckUserExistsQuery, bool>
{
    private readonly ISystemUserRepository _userRepository;

    public CheckUserExistsQueryHandler(ISystemUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> HandleAsync(CheckUserExistsQuery query, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsAsync(query.Email, cancellationToken);
    }
}
