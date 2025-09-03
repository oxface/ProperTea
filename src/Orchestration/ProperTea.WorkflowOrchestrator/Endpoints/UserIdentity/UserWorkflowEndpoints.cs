using Microsoft.AspNetCore.Mvc;
using ProperTea.WorkflowOrchestrator.Services;

namespace ProperTea.WorkflowOrchestrator.Endpoints.UserIdentity;

public static class UserWorkflowEndpoints
{
    public static void MapUserWorkflowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflows/users")
            .WithTags("User Workflows")
            .RequireAuthorization();
        
        CreateUserWithIdentityEndpoint.Map(app);
    }
}

public static class CreateUserWithIdentityEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workflows/users", HandleAsync)
            .WithName("CreateUserWithIdentity")
            .WithSummary("Create a new user with identity")
            .WithDescription("Creates a new user in UserManagement and corresponding identity in Identity service")
            .WithTags("User Workflows")
            .Produces<CreateUserWithIdentityResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateUserWithIdentityRequest request,
        IGatewayClient gatewayClient,
        ILogger<Program> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting user with identity creation workflow for {Email}", request.Email);

        Guid? userId = null;
        var userCreated = false;

        try
        {
            var existingUser = await gatewayClient.GetAsync<UserExistsResponse>(
                $"/api/users/exists?email={Uri.EscapeDataString(request.Email)}", 
                cancellationToken);

            if (existingUser?.Exists == true)
            {
                logger.LogWarning("User already exists for email {Email}", request.Email);
                return Results.BadRequest(new { Message = $"User with email {request.Email} already exists" });
            }
            
            logger.LogInformation("Creating user in UserManagement service for {Email}", request.Email);
            
            var userResponse = await gatewayClient.PostAsync<CreateUserRequest, CreateUserResponse>(
                "/api/users", 
                new CreateUserRequest(request.Email, request.FullName), 
                cancellationToken);

            if (userResponse == null)
            {
                throw new InvalidOperationException("Failed to create user in UserManagement service");
            }

            userId = userResponse.UserId;
            userCreated = true;
            logger.LogInformation("User created successfully: {UserId}", userId);
            
            if (!string.IsNullOrEmpty(request.Password))
            {
                logger.LogInformation("Creating identity for user {UserId}", userId);
                
                var identityResponse = await gatewayClient.PostAsync<CreateIdentityRequest, CreateIdentityResponse>(
                    "/api/identities", 
                    new CreateIdentityRequest(userId.Value, request.Email, request.Password), 
                    cancellationToken);

                if (identityResponse == null)
                {
                    throw new InvalidOperationException("Failed to create identity for user");
                }

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
            logger.LogError(ex, "User with identity creation failed for {Email}, attempting compensation", request.Email);
            
            if (userCreated && userId.HasValue)
            {
                try
                {
                    await gatewayClient.PostAsync<object, object>(
                        $"/api/users/{userId}/delete", 
                        new { }, 
                        cancellationToken);
                    logger.LogInformation("Successfully compensated user creation for {UserId}", userId);
                }
                catch (Exception compensationEx)
                {
                    logger.LogError(compensationEx, "Failed to compensate user creation for {UserId}", userId);
                }
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
