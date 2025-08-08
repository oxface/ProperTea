using ProperTea.Shared.Application.Commands;
using ProperTea.SystemUser.Application.Commands;

namespace ProperTea.SystemUser.Api.Endpoints;

public static class CreateSystemUserEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/system-owner",
            async (CreateSystemUserCommand command, ICommandHandler<CreateSystemUserCommand, Guid> handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/companies/{result}", result);
            });
    }
}