using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Domain.Users;

namespace ProperTea.UserManagement.Application.Users.Queries;

public record CheckUserExistsQuery(string Email) : IQuery;

public class CheckUserExistsQueryHandler : IQueryHandler<CheckUserExistsQuery, bool>
{
    private readonly IUserRepository userRepository;

    public CheckUserExistsQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<bool> HandleAsync(CheckUserExistsQuery query, CancellationToken ct = default)
    {
        return await userRepository.ExistsAsync(query.Email, ct);
    }
}