using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Models;
using ProperTea.Organization.Api.Application.Queries;

namespace ProperTea.Organization.Api.Endpoints;

public static class GetOrganizationByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/organizations/{id}", HandleAsync)
            .WithName("GetOrganizationById")
            .WithSummary("Get organization by ID")
            .WithDescription("Retrieves an organization by its unique identifier")
            .WithTags("Organizations")
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
            var query = new GetOrganizationByIdQuery(id);
            var result = await queryBus.SendAsync<GetOrganizationByIdQuery, OrganizationModel?>(query);

            if (result == null)
            {
                logger.LogWarning("Organization not found: {Id}", id);
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting organization: {Id}", id);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }
}