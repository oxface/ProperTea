using Microsoft.AspNetCore.Mvc;
using ProperTea.Contracts.CQRS;
using ProperTea.UserManagement.Api.Application.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class GetUserByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id}", HandleAsync)
            .WithName("GetUserById")
            .WithSummary("Get user by ID")
            .WithDescription("Retrieves a user by their unique identifier")
            .WithTags("Users")
            .Produces<object>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var result = await queryBus.SendAsync(query);

            if (result == null)
            {
                logger.LogWarning("User not found: {Id}", id);
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user: {Id}", id);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}
