using ProperTea.Organization.Application.Models;
using ProperTea.Organization.Application.Queries;
using ProperTea.Organization.Domain;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Organization.Api.Endpoints;

public static class GetOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/organization",
            async (HttpRequest request, IQueryHandler<GetOrganizationByFilterQuery, PagedResult<OrganizationModel>> handler) =>
            {
                var filter = new OrganizationFilter
                {
                    Name = request.Query["name"]
                };
                var query = new GetOrganizationByFilterQuery
                {
                    Filter = filter
                };
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            });
    }
}