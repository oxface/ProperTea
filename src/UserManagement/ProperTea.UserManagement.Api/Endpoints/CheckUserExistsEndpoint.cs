using Microsoft.AspNetCore.Mvc;
using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Application.Users.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class CheckUserExistsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/exists", HandleAsync)
            .WithName("CheckUserExists")
            .WithSummary("Check if user exists")
            .WithDescription("Checks whether a user exists by their email address")
            .WithTags("UserIds")
            .Produces<bool>()
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromQuery] string email,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var query = new CheckUserExistsQuery(email);
            var exists = await queryBus.SendAsync<CheckUserExistsQuery, bool>(query);

            return Results.Ok(
                new
                {
                    Exists = exists
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking user exists: {Email}", email);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}