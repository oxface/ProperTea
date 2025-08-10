using ProperTea.Organization.Application.Models;
using ProperTea.Organization.Application.Queries;
using ProperTea.Shared.Application.Queries;

namespace ProperTea.Organization.Api.Endpoints;

public static class GetOrganizationByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/organization/{id:guid}",
            async (Guid id, IQueryHandler<GetOrganizationByIdQuery, OrganizationModel> handler) =>
            {
                var result = await handler.HandleAsync(
                    new GetOrganizationByIdQuery
                    {
                        Id = id
                    });
                return Results.Ok(result);
            });
    }
}