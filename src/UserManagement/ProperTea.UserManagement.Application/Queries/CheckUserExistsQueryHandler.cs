using ProperTea.Cqrs;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Application.Queries;

public record CheckUserExistsQuery(string Email) : IQuery;

public class CheckUserExistsQueryHandler : IQueryHandler<CheckUserExistsQuery, bool>
{
    private readonly ISystemUserRepository userRepository;

    public CheckUserExistsQueryHandler(ISystemUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<bool> HandleAsync(CheckUserExistsQuery query, CancellationToken ct = default)
    {
        return await userRepository.ExistsAsync(query.Email, ct);
    }
}