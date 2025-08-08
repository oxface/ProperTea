using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Application.Commands;

namespace ProperTea.SystemOwner.Api.Endpoints;

public static class CreateSystemOwnerEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/system-owner",
            async (CreateSystemOwnerCommand command, ICommandHandler<CreateSystemOwnerCommand, Guid> handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/companies/{result}", result);
            });
    }
}