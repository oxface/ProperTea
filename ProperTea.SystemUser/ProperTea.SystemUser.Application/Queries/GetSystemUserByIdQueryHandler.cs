using ProperTea.Shared.Application.Queries;
using ProperTea.SystemUser.Application.Models;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Application.Queries;

public class GetSystemUserByIdQueryHandler(ISystemUserRepository repository)
    : IQueryHandler<GetSystemUserByIdQuery, SystemUserModel>
{
    public async Task<SystemUserModel> HandleAsync(GetSystemUserByIdQuery query, CancellationToken ct)
    {
        var systemUser = await repository.GetByIdAsync(query.Id, ct);
        if (systemUser == null)
            throw new Exception("SystemUser not found");
        return new SystemUserModel
        {
            Id = systemUser.Id,
            Name = systemUser.Name
        };
    }
}