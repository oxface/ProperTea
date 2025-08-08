using ProperTea.Shared.Application.Commands;
using ProperTea.SystemOwner.Application.Commands;

namespace ProperTea.SystemOwner.Api.Endpoints;

public static class ChangeSystemOwnerNameEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/system-owner/{id:guid}/name",
            async (ChangeSystemOwnerNameCommand command, Guid id, ICommandHandler<ChangeSystemOwnerNameCommand> handler) =>
            {
                command.Id = id;
                await handler.HandleAsync(command);
                return Results.NoContent();
            });
    }
}