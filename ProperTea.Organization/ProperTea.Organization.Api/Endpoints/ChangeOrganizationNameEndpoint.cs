using ProperTea.Organization.Application.Commands;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Api.Endpoints;

public static class ChangeOrganizationNameEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/organization/{id:guid}/name",
            async (ChangeOrganizationNameCommand command, Guid id, ICommandHandler<ChangeOrganizationNameCommand> handler) =>
            {
                command.Id = id;
                await handler.HandleAsync(command);
                return Results.NoContent();
            });
    }
}