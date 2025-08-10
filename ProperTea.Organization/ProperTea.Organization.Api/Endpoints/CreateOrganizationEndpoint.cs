using ProperTea.Organization.Application.Commands;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Api.Endpoints;

public static class CreateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/organization",
            async (CreateOrganizationCommand command, ICommandHandler<CreateOrganizationCommand, Guid> handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/companies/{result}", result);
            });
    }
}