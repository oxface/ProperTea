using Microsoft.AspNetCore.Mvc;
using ProperTea.ProperCqrs;
using ProperTea.UserManagement.Api.Models;
using ProperTea.UserManagement.Application.Users.Commands;
using ProperTea.UserManagement.Application.Users.Models;
using ProperTea.UserManagement.Application.Users.Queries;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class CreateUserEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", HandleAsync)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new system user with email and full name")
            .WithTags("UserIds")
            .Produces<object>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateUserRequest request,
        ICommandBus commandBus,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new CreateUserCommand(request.Email, request.FullName);
            await commandBus.SendAsync(command);

            var query = new GetUserByEmailQuery(request.Email);
            var user = await queryBus.SendAsync<GetUserByEmailQuery, UserModel>(query);

            return Results.Ok(new IdResponse(user!.Id));
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

public record CreateUserRequest(string Email, string FullName);