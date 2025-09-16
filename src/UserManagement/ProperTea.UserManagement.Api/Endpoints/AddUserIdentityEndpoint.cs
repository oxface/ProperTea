using Microsoft.AspNetCore.Mvc;
using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Application.Users.Commands;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class AddUserIdentityEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/add-identity", HandleAsync)
            .WithName("AddUserIdentity")
            .WithSummary("Adds an identity to the user")
            .WithTags("UserIds")
            .Produces<object>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AddUserIdentityRequest request,
        ICommandBus commandBus,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new AddUserIdentityCommand(request.UserId, request.IdentityId);
            await commandBus.SendAsync(command);

            return Results.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding identity to user: {id}", request.UserId);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}

public record AddUserIdentityRequest(Guid UserId, Guid IdentityId);