using ProperTea.Shared.Application.Queries;
using ProperTea.SystemOwner.Application.Models;
using ProperTea.SystemOwner.Application.Queries;

namespace ProperTea.SystemOwner.Api.Endpoints;

public static class GetSystemOwnerByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/system-owner/{id:guid}",
            async (Guid id, IQueryHandler<GetSystemOwnerByIdQuery, SystemOwnerModel> handler) =>
            {
                var result = await handler.HandleAsync(new GetSystemOwnerByIdQuery
                {
                    Id = id
                });
                return Results.Ok(result);
            });
    }
}