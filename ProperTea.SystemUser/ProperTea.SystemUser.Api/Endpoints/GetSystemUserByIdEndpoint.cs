using ProperTea.Shared.Application.Queries;
using ProperTea.SystemUser.Application.Models;
using ProperTea.SystemUser.Application.Queries;

namespace ProperTea.SystemUser.Api.Endpoints;

public static class GetSystemUserByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/system-owner/{id:guid}",
            async (Guid id, IQueryHandler<GetSystemUserByIdQuery, SystemUserModel> handler) =>
            {
                var result = await handler.HandleAsync(
                    new GetSystemUserByIdQuery
                    {
                        Id = id
                    });
                return Results.Ok(result);
            });
    }
}