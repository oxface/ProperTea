using Microsoft.AspNetCore.Mvc;

namespace ProperTea.Landlord.Bff.Endpoints.Organization;

public static class CreateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/organizations", HandleAsync)
            .WithName("CreateOrganization")
            .WithSummary("Create a new organization")
            .WithDescription("Creates a new organization directly through the organization service")
            .WithTags("Organizations")
            .Produces<CreateOrganizationResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateOrganizationRequest request,
        IHttpClientFactory httpClientFactory,
        ILogger<Program> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating organization: {Name}", request.Name);

        var gatewayClient = httpClientFactory.CreateClient("gateway");
        try
        {
            var response = await gatewayClient.PostAsJsonAsync(
                "/api/organizations", 
                request, 
                cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to create organization via Gateway");
                return Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Failed to create organization",
                    detail: "The organization service did not return a valid response");
            }

            var result = await response.Content.ReadFromJsonAsync<CreateOrganizationResponse>(cancellationToken);
            logger.LogInformation("Organization created successfully: {OrganizationId}", result!.OrganizationId);
            return Results.Ok(result);
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

public record CreateOrganizationRequest(
    string Name,
    string Description);

public record CreateOrganizationResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public Guid? OrganizationId { get; init; }
}
