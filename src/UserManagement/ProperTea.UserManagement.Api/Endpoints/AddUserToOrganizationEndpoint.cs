using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.UserManagement.Application.Commands;
using ProperTea.UserManagement.Domain.SystemUsers;

namespace ProperTea.UserManagement.Api.Endpoints;

public static class AddUserToOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/add-to-organization", HandleAsync)
            .WithName("AddUserToOrganization")
            .WithSummary("Add user to organization")
            .WithDescription("Adds a user to an organization with a specific role")
            .WithTags("Users")
            .Produces<object>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AddUserToOrganizationApiRequest request,
        ICommandBus commandBus,
        ILogger<Program> logger)
    {
        try
        {
            if (!Enum.TryParse<SystemUserOrganizationRole>(request.Role, out var role))
            {
                logger.LogWarning("Invalid role provided: {Role}", request.Role);
                return Results.BadRequest("Invalid role");
            }

            var command = new AddUserToOrganizationCommand(request.UserId, request.OrganizationId, role);
            await commandBus.SendAsync(command);

            return Results.Ok(new { Message = "User added to organization successfully" });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation when adding user {UserId} to organization {OrganizationId}",
                request.UserId, request.OrganizationId);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding user {UserId} to organization {OrganizationId}",
                request.UserId, request.OrganizationId);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}

public record AddUserToOrganizationApiRequest(Guid UserId, Guid OrganizationId, string Role);