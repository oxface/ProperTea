using Microsoft.AspNetCore.Mvc;
using ProperTea.Contracts.CQRS;
using ProperTea.Organization.Api.Application.Commands;
using ProperTea.Organization.Api.DTOs;

namespace ProperTea.Organization.Api.Endpoints;

public static class CreateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/organizations", HandleAsync)
            .WithName("CreateOrganization")
            .WithSummary("Create a new organization")
            .WithDescription("Creates a new organization with the specified details")
            .WithTags("Organizations")
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateOrganizationApiRequest request,
        ICommandBus commandBus,
        ILogger<Program> logger)
    {
        try
        {
            var command = new CreateOrganizationCommand(request.Name, request.Description, request.CreatedByUserId);
            await commandBus.SendAsync(command);

            return Results.Ok(new { Message = "Organization created successfully" });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Invalid operation when creating organization: {Name}", request.Name);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating organization: {Name}", request.Name);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}
