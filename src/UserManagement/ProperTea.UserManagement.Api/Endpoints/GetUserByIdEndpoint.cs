using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Application.Users.Models;
using ProperTea.UserManagement.Application.Users.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class GetUserByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id}", HandleAsync)
            .WithName("GetUserById")
            .WithSummary("Get user by ID")
            .WithDescription("Retrieves a user by their unique identifier")
            .WithTags("UserIds")
            .Produces<object>()
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
            var result = await queryBus.SendAsync<GetUserByIdQuery, UserModel>(query);

            if (result == null)
            {
                logger.LogWarning("User not found: {EventId}", id);
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user: {EventId}", id);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}