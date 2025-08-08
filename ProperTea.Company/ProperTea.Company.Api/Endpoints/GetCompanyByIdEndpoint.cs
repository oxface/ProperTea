using ProperTea.Company.Application.Models;
using ProperTea.Company.Application.Queries;
using ProperTea.Shared.Application.Queries;

namespace ProperTea.Company.Api.Endpoints;

public static class GetCompanyByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/company/{id:guid}",
            async (Guid id, IQueryHandler<GetCompanyByIdQuery, CompanyModel> handler) =>
            {
                var result = await handler.HandleAsync(
                    new GetCompanyByIdQuery
                    {
                        Id = id
                    });
                return Results.Ok(result);
            });
    }
}