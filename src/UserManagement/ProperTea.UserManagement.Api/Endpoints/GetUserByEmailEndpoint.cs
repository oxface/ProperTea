using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class GetUserByEmailEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/by-email", HandleAsync)
            .WithName("GetUserByEmail")
            .WithSummary("Get user by email")
            .WithDescription("Retrieves a user by their email address")
            .WithTags("Users")
            .Produces<object>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromQuery] string email,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var query = new GetUserByEmailQuery(email);
            var result = await queryBus.SendAsync<GetUserByEmailQuery, SystemUserModel>(query);

            if (result == null)
            {
                logger.LogWarning("User not found by email: {Email}", email);
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user by email: {Email}", email);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}