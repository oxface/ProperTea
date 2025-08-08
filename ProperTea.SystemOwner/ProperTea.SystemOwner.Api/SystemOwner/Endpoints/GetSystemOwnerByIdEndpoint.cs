using ProperTea.Shared.Application.Queries;
using ProperTea.SystemOwner.Application.SystemOwner.Models;
using ProperTea.SystemOwner.Application.SystemOwner.Queries;

namespace ProperTea.SystemOwner.Api.SystemOwner.Endpoints;

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