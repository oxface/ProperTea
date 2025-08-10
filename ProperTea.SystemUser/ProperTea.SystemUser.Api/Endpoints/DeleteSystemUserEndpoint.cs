using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Application.Commands;

namespace ProperTea.SystemUser.Api.Endpoints;

public static class DeleteSystemUserEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
            "/system-user/{id:guid}",
            async (Guid id, ICommandHandler<DeleteSystemUserCommand> handler) =>
            {
                await handler.HandleAsync(
                    new DeleteSystemUserCommand
                    {
                        Id = id
                    });
                return Results.NoContent();
            });
    }
}