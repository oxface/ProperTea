using ProperTea.Company.Application.Commands;
using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Api.Endpoints;

public static class CreateCompanyEndpoint
{
    public static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/company",
            async (CreateCompanyCommand command, ICommandHandler<CreateCompanyCommand, Guid> handler) =>
            {
                var result = await handler.HandleAsync(command);
                return Results.Created($"/company/{result}", result);
            });
    }
}