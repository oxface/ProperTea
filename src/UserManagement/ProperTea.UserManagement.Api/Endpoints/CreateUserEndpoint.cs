using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Api.Application.Commands;
using ProperTea.UserManagement.Api.Application.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class CreateUserEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", HandleAsync)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new system user with email and full name")
            .WithTags("Users")
            .Produces<object>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateUserApiRequest request,
        ICommandBus commandBus,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new CreateSystemUserCommand(request.Email, request.FullName);
            await commandBus.SendAsync(command);

            var query = new GetUserByEmailQuery(request.Email);
            var user = await queryBus.SendAsync<GetUserByEmailQuery, SystemUserModel>(query);

            return Results.Ok(new { UserId = user!.Id });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation when creating user: {Email}", request.Email);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user: {Email}", request.Email);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}

public record CreateUserApiRequest(string Email, string FullName);