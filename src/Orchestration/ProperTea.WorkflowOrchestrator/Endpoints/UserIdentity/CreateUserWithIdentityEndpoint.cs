using Microsoft.AspNetCore.Mvc;
using ProperTea.Infrastructure.Shared.Extensions;

namespace ProperTea.WorkflowOrchestrator.Endpoints.UserIdentity;

public static class CreateUserWithIdentityEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workflows/users", HandleAsync)
            .WithName("CreateUserWithIdentity")
            .WithSummary("Create a new user with identity")
            .WithDescription("Creates a new user in UserManagement and corresponding identity in Identity service")
            .WithTags("User Workflows")
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
        logger.LogInformation("Starting user with identity creation workflow for {Email}", request.Email);

        var gatewayClient = httpClientFactory.CreateClient("gateway");

        Guid? userId = null;
        var userCreated = false;

        try
        {
            var userExistsResponse = await gatewayClient.GetAsync<UserExistsResponse>(
                $"/api/users/exists?email={Uri.EscapeDataString(request.Email)}",
                logger,
                cancellationToken);

            if (userExistsResponse?.Exists == true)
            {
                logger.LogWarning("User already exists for email {Email}", request.Email);
                return Results.BadRequest(new { Message = $"User with email {request.Email} already exists" });
            }

            logger.LogInformation("Creating user in UserManagement service for {Email}", request.Email);

            var createUserRequest = new CreateUserRequest(request.Email, request.FullName);
            var createUserResponse = await gatewayClient.PostAsync<CreateUserRequest, CreateUserResponse>(
                "/api/users",
                createUserRequest,
                logger,
                cancellationToken);

            if (createUserResponse == null)
                throw new InvalidOperationException("Failed to create user");

            userId = createUserResponse!.UserId;
            userCreated = true;
            logger.LogInformation("User created successfully: {UserId}", userId);

            if (!string.IsNullOrEmpty(request.Password))
            {
                logger.LogInformation("Creating identity for user {UserId}", userId);

                var createIdentityRequest = new CreateIdentityRequest(userId.Value, request.Email, request.Password);
                var createIdentityResponse =
                    await gatewayClient.PostAsync<CreateIdentityRequest, CreateIdentityResponse>(
                        "/api/identities",
                        createIdentityRequest,
                        logger,
                        cancellationToken);

                if (createIdentityResponse == null)
                    throw new InvalidOperationException("Failed to create identity for user");

                logger.LogInformation("Identity created successfully for user {UserId}", userId);
            }

            logger.LogInformation("User with identity creation completed for {Email} with ID {UserId}",
                request.Email, userId);

            return Results.Ok(new CreateUserWithIdentityResponse(
                userId.Value,
                request.Email,
                request.FullName,
                !string.IsNullOrEmpty(request.Password)));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User with identity creation failed for {Email}, attempting compensation",
                request.Email);

            if (userCreated && userId.HasValue)
                try
                {
                    await gatewayClient.PostAsync<object, object>(
                        $"/api/users/{userId}/delete",
                        new { },
                        logger,
                        cancellationToken);
                    logger.LogInformation("Successfully compensated user creation for {UserId}", userId);
                }
                catch (Exception compensationEx)
                {
                    logger.LogError(compensationEx, "Failed to compensate user creation for {UserId}", userId);
                }

            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "User creation failed",
                detail: ex.Message);
        }
    }
}

public record CreateUserWithIdentityRequest(
    string Email,
    string FullName,
    string? Password = null);

public record CreateUserWithIdentityResponse(
    Guid UserId,
    string Email,
    string FullName,
    bool IdentityCreated);

public record CreateUserRequest(string Email, string FullName);

public record CreateUserResponse(Guid UserId, string Email, string FullName);

public record CreateIdentityRequest(Guid UserId, string Email, string Password);

public record CreateIdentityResponse(Guid IdentityId, Guid UserId);

public record UserExistsResponse(bool Exists);