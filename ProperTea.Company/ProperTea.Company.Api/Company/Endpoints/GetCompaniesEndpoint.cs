using ProperTea.Company.Application.Company.Models;
using ProperTea.Company.Application.Company.Queries;
using ProperTea.Company.Domain.Company;
using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;

namespace ProperTea.Company.Api.Company.Endpoints;

public static class GetCompaniesEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/company",
            async (HttpRequest request, IQueryHandler<GetCompaniesByFilterQuery, PagedResult<CompanyModel>> handler) =>
            {
                var filter = new CompanyFilter
                {
                    Name = request.Query["name"]
                };
                var query = new GetCompaniesByFilterQuery
                {
                    Filter = filter
                };
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            });
    }
}