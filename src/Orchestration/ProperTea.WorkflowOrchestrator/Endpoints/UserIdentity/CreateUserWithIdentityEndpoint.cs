using Microsoft.AspNetCore.Mvc;
using ProperTea.Shared.Infrastructure.Extensions;
using ProperTea.WorkflowOrchestrator.Models;

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
        Guid? identityId = null;
        var identityCreated = false;

        try
        {
            // Create user.
            var createUserRequest = new CreateUserRequest(request.Email, request.FullName);
            var createUserResponse = await gatewayClient.PostAsync<CreateUserRequest, IdResponse>(
                "/api/users",
                createUserRequest,
                logger,
                cancellationToken);

            if (createUserResponse == null)
                throw new InvalidOperationException("Failed to create user");

            userId = createUserResponse!.Id;
            userCreated = true;
            logger.LogInformation("User created successfully: {UserId}", userId);

            // Create user identity with user-defined credentials.
            if (!string.IsNullOrEmpty(request.Password))
            {
                logger.LogInformation("Creating identity for user {UserId}", userId);

                var createIdentityRequest = new CreateIdentityRequest(userId.Value, request.Email, request.Password);
                var createIdentityResponse =
                    await gatewayClient.PostAsync<CreateIdentityRequest, IdResponse>(
                        "/api/identities",
                        createIdentityRequest,
                        logger,
                        cancellationToken);

                if (createIdentityResponse == null)
                    throw new InvalidOperationException("Failed to create identity for user");

                identityId = createIdentityResponse!.Id;
                identityCreated = true;
                logger.LogInformation("Identity created successfully for user {UserId}", userId);
            }

            // Add existing user identity to user.
            var addUserIdentityRequest = new AddUserIdentityRequest(userId!.Value, identityId!.Value);
            await gatewayClient.PostAsync(
                "/api/users/add-identity",
                addUserIdentityRequest,
                logger,
                cancellationToken);

            logger.LogInformation("User with identity creation completed for {Email} with ID {UserId}",
                request.Email, userId);

            return Results.Ok();
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

            if (identityCreated && identityId.HasValue)
                try
                {
                    // await gatewayClient.PostAsync<object, object>(
                    //     $"/api/users/{userId}/delete",
                    //     new { },
                    //     logger,
                    //     cancellationToken);
                    // logger.LogInformation("Successfully compensated user creation for {UserId}", userId);
                }
                catch (Exception compensationEx)
                {
                    logger.LogError(compensationEx, "Failed to compensate user identity creation for {identityId}",
                        identityId);
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

public record CreateUserRequest(string Email, string FullName);

public record CreateIdentityRequest(Guid UserId, string Email, string Password);

public record AddUserIdentityRequest(Guid UserId, Guid IdentityId);