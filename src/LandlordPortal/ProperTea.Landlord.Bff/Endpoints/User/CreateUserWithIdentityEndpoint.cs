using Microsoft.AspNetCore.Mvc;
using ProperTea.Infrastructure.Shared.Extensions;

namespace ProperTea.Landlord.Bff.Endpoints.User;

public static class CreateUserWithIdentityEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", HandleAsync)
            .WithName("CreateUser")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with identity through workflow orchestrator")
            .WithTags("Users")
            .Produces<CreateUserWithIdentityResponse>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateUserWithIdentityRequest request,
        IHttpClientFactory httpClientFactory,
        ILogger<Program> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating user with identity: {Email}", request.Email);

        var gatewayClient = httpClientFactory.CreateClient("gateway");
        try
        {
            var response = await gatewayClient.PostAsync<CreateUserWithIdentityRequest, CreateUserWithIdentityResponse>(
                "/api/workflows/users",
                request,
                logger,
                cancellationToken);

            logger.LogInformation("User created successfully: {UserId}", response!.UserId);
            return Results.Ok(response);
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

public record CreateUserWithIdentityRequest(
    string Email,
    string FullName,
    string Password);

public record CreateUserWithIdentityResponse(
    Guid UserId);