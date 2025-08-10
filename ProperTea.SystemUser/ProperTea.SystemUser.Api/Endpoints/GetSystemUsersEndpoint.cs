using ProperTea.Shared.Application.Queries;
using ProperTea.Shared.Domain.Pagination;
using ProperTea.SystemUser.Application.Models;
using ProperTea.SystemUser.Application.Queries;
using ProperTea.SystemUser.Domain;

namespace ProperTea.SystemUser.Api.Endpoints;

public static class GetSystemUsersEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/system-user",
            async (HttpRequest request, IQueryHandler<GetSystemUsersByFilterQuery, PagedResult<SystemUserModel>> handler) =>
            {
                var filter = new SystemUserFilter
                {
                    Name = request.Query["name"]
                };
                var query = new GetSystemUsersByFilterQuery
                {
                    Filter = filter
                };
                var result = await handler.HandleAsync(query);
                return Results.Ok(result);
            });
    }
}