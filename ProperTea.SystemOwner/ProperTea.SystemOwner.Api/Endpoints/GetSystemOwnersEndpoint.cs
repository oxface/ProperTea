using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemOwner.Application.Models;
using ProperTea.SystemOwner.Application.Queries;
using ProperTea.SystemOwner.Domain;

namespace ProperTea.SystemOwner.Api.Endpoints;

public static class GetSystemOwnersEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/system-owner",
            async (HttpRequest request, IQueryHandler<GetSystemOwnersByFilterQuery, PagedResult<SystemOwnerModel>> handler) =>
            {
                var filter = new SystemOwnerFilter
                {
                    Name = request.Query["name"]
                };
                var query = new GetSystemOwnersByFilterQuery
                {
                    Filter = filter
                };
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            });
    }
}