using Microsoft.AspNetCore.Mvc;
using ProperTea.Contracts.CQRS;
using ProperTea.Organization.Api.Application.Queries;

namespace ProperTea.Organization.Api.Endpoints;

public static class CheckOrganizationExistsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/organizations/{id}/exists", HandleAsync)
            .WithName("CheckOrganizationExists")
            .WithSummary("Check if organization exists")
            .WithDescription("Checks whether an organization exists by its ID")
            .WithTags("Organizations")
            .Produces<bool>(StatusCodes.Status200OK)
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
            var query = new CheckOrganizationExistsQuery(id);
            var exists = await queryBus.SendAsync(query);

            return Results.Ok(exists);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking organization exists: {Id}", id);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}
