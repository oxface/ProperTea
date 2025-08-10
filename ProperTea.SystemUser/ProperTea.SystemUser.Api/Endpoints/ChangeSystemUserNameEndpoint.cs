using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Application.Commands;

namespace ProperTea.SystemUser.Api.Endpoints;

public static class ChangeSystemUserNameEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
            "/system-user/{id:guid}/name",
            async (ChangeSystemUserNameCommand command, Guid id, ICommandHandler<ChangeSystemUserNameCommand> handler) =>
            {
                command.Id = id;
                await handler.HandleAsync(command);
                return Results.NoContent();
            });
    }
}