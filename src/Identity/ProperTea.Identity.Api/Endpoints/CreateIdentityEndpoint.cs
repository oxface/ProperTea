using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.Identity.Api.Application.Commands;

namespace ProperTea.Identity.Api.Endpoints;

public static class CreateIdentityEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identities", HandleAsync)
            .WithName("CreateIdentity")
            .WithSummary("Create a new identity")
            .WithDescription("Creates a new identity with email and password credentials")
            .WithTags("Identities")
            .Produces<object>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateIdentityApiRequest request,
        ICommandBus commandBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new CreateIdentityCommand(request.UserId, request.Email, request.Password);
            await commandBus.SendAsync(command);

            return Results.Ok(new { Message = "Identity created successfully" });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation when creating identity for user: {UserId}", request.UserId);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating identity for user: {UserId}", request.UserId);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}

public record CreateIdentityApiRequest(Guid UserId, string Email, string Password);