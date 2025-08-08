using ProperTea.Shared.Application.Queries;
using ProperTea.SystemOwner.Application.Models;
using ProperTea.SystemOwner.Domain;

namespace ProperTea.SystemOwner.Application.Queries;

public class GetSystemOwnerByIdQueryHandler(ISystemOwnerRepository repository)
    : IQueryHandler<GetSystemOwnerByIdQuery, SystemOwnerModel>
{
    public async Task<SystemOwnerModel> HandleAsync(GetSystemOwnerByIdQuery query, CancellationToken ct)
    {
        var systemOwner = await repository.GetByIdAsync(query.Id, ct);
        if (systemOwner == null)
            throw new Exception("SystemOwner not found");
        return new SystemOwnerModel
        {
            Id = systemOwner.Id,
            Name = systemOwner.Name
        };
    }
}