using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.Identity.Api.Application.Commands;

namespace ProperTea.Identity.Api.Endpoints;

public static class AuthenticateEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identities/authenticate", HandleAsync)
            .WithName("Authenticate")
            .WithSummary("Authenticate user")
            .WithDescription("Authenticates a user with email and password")
            .WithTags("Identities")
            .Produces<object>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .AllowAnonymous(); // Authentication endpoint should allow anonymous access
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AuthenticateApiRequest request,
        ICommandBus commandBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new AuthenticateUserCommand(request.Email, request.Password);
            await commandBus.SendAsync(command);

            return Results.Ok(new { Message = "Authentication successful" });
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning("Authentication failed for email: {Email}", request.Email);
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error authenticating user with email: {Email}", request.Email);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}

public record AuthenticateApiRequest(string Email, string Password);