using ProperTea.Company.Application.Commands;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Api.Endpoints;

public static class DeleteCompanyEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/company/{id:guid}",
            async (Guid id, ICommandHandler<DeleteCompanyCommand> handler) =>
            {
                await handler.HandleAsync(
                    new DeleteCompanyCommand
                    {
                        Id = id
                    });
                return Results.NoContent();
            });
    }
}