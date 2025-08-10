using ProperTea.Organization.Application.Commands;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Api.Endpoints;

public static class DeleteOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/organization/{id:guid}",
            async (Guid id, ICommandHandler<DeleteOrganizationCommand> handler) =>
            {
                await handler.HandleAsync(
                    new DeleteOrganizationCommand
                    {
                        Id = id
                    });
                return Results.NoContent();
            });
    }
}