using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Commands;

namespace ProperTea.Organization.Api.Endpoints;

public static class ActivateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/organizations/{id}/activate", HandleAsync)
            .WithName("ActivateOrganization")
            .WithSummary("Activate an organization")
            .WithDescription("Activates an organization by its ID")
            .WithTags("Organizations")
            .Produces<object>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        ICommandBus commandBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new ActivateOrganizationCommand(id);
            await commandBus.SendAsync(command);

            return Results.Ok(new { Message = "Organization activated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation when activating organization: {Id}", id);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error activating organization: {Id}", id);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}