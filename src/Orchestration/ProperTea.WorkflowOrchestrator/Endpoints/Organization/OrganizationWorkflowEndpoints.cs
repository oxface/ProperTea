using Microsoft.AspNetCore.Mvc;
using ProperTea.Infrastructure.Shared.Extensions;

namespace ProperTea.WorkflowOrchestrator.Endpoints.Organization;

public static class OrganizationWorkflowEndpoints
{
    public static void MapOrganizationWorkflowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflows/organizations")
            .WithTags("Organization Workflows")
            .RequireAuthorization();

        CreateOrganizationEndpoint.Map(app);
    }
}

public static class CreateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workflows/organizations", HandleAsync)
            .WithName("CreateOrganization")
            .WithSummary("Create a new organization")
            .WithDescription("Creates a new organization through the organization service")
            .WithTags("Organization Workflows")
            .Produces<CreateOrganizationWorkflowResponse>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateOrganizationWorkflowRequest request,
        IHttpClientFactory httpClientFactory,
        ILogger<Program> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting organization creation workflow for {Name}", request.Name);

        try
        {
            var gatewayClient = httpClientFactory.CreateClient("gateway");

            var response = await gatewayClient.PostAsync<CreateOrganizationRequest, CreateOrganizationResponse>(
                "/api/organizations",
                new CreateOrganizationRequest(request.Name, request.Description),
                logger,
                cancellationToken);

            if (response == null)
            {
                logger.LogError("Failed to create organization via Gateway");
                return Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Failed to create organization",
                    detail: "The organization service did not return a valid response");
            }

            logger.LogInformation("Organization created successfully: {OrganizationId}", response.OrganizationId);

            return Results.Ok(new CreateOrganizationWorkflowResponse(
                response.OrganizationId,
                response.Name,
                response.Description));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating organization: {Name}", request.Name);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error",
                detail: ex.Message);
        }
    }
}

public record CreateOrganizationWorkflowRequest(
    string Name,
    string Description);

public record CreateOrganizationWorkflowResponse(
    Guid OrganizationId,
    string Name,
    string Description);

public record CreateOrganizationRequest(string Name, string Description);

public record CreateOrganizationResponse(Guid OrganizationId, string Name, string Description);