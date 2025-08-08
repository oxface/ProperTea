using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Application.SystemOwner.Commands;

namespace ProperTea.SystemOwner.Api.SystemOwner.Endpoints;

public static class DeleteSystemOwnerEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/system-owner/{id:guid}",
            async (Guid id, ICommandHandler<DeleteSystemOwnerCommand> handler) =>
            {
                await handler.HandleAsync(new DeleteSystemOwnerCommand
                {
                    Id = id
                });
                return Results.NoContent();
            });
    }
}