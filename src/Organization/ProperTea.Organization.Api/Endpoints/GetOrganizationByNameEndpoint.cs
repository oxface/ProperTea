using Microsoft.AspNetCore.Mvc;
using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Models;
using ProperTea.Organization.Api.Application.Queries;

namespace ProperTea.Organization.Api.Endpoints;

public static class GetOrganizationByNameEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/organizations/by-name", HandleAsync)
            .WithName("GetOrganizationByName")
            .WithSummary("Get organization by name")
            .WithDescription("Retrieves an organization by its name")
            .WithTags("Organizations")
            .Produces<object>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromQuery] string name,
        IQueryBus queryBus,
        ILogger<Program> logger)
    {
        try
        {
            var query = new GetOrganizationByNameQuery(name);
            var result = await queryBus.SendAsync<GetOrganizationByNameQuery, OrganizationModel?>(query);

            if (result == null)
            {
                logger.LogWarning("Organization not found by name: {Name}", name);
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting organization by name: {Name}", name);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}