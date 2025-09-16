using ProperTea.ProperCqrs;
using ProperTea.Service.Application.AggregateRootNames.Models;
using ProperTea.Service.Domain.AggregateRootNames;

namespace ProperTea.Service.Application.AggregateRootNames.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery;

public class GetUserByIdQueryHandler(IAggregateRootNameRepository repository)
    : IQueryHandler<GetUserByIdQuery, AggregateRootNameModel?>
{
    public async Task<AggregateRootNameModel?> HandleAsync(GetUserByIdQuery query,
        CancellationToken ct = default)
    {
        var user = await repository.GetByIdAsync(query.UserId, ct);
        return user == null ? null : AggregateRootNameModel.FromEntity(user);
    }
}