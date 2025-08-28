using Microsoft.AspNetCore.Mvc;
using ProperTea.Contracts.DTOs.Organization;
using ProperTea.Landlord.Bff.Services;
using System.Text.Json;

namespace ProperTea.Landlord.Bff.Endpoints;

public static class CreateOrganizationEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/organizations", HandleAsync)
            .WithName("CreateOrganization")
            .WithSummary("Create a new organization")
            .WithDescription("Creates a new organization and waits for completion")
            .WithTags("Organizations")
            .Produces<CreateOrganizationResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateOrganizationRequest request,
        IGatewayClient gatewayClient,
        IConfiguration configuration,
        ILogger<Program> logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating organization synchronously: {Name}", request.Name);

        try
        {
            // Start the orchestration via Gateway
            var orchestrationResponse = await gatewayClient.PostAsync<CreateOrganizationRequest, OrchestrationStatusResponse>(
                "/api/orchestrators/CreateOrganizationStarter", 
                request, 
                cancellationToken);
            
            if (orchestrationResponse == null || string.IsNullOrEmpty(orchestrationResponse.Id))
            {
                logger.LogError("Failed to start organization creation via Gateway");
                return Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Failed to start organization creation",
                    detail: "The Gateway did not return a valid orchestration response");
            }

            logger.LogInformation("Orchestration started with ID: {InstanceId}, waiting for completion...", orchestrationResponse.Id);

            // Wait for orchestration completion with configurable timeout
            var timeoutSeconds = configuration.GetValue("OrganizationCreation:TimeoutSeconds", 30);
            var pollingIntervalMs = configuration.GetValue("OrganizationCreation:PollingIntervalMs", 1000);
            
            var result = await WaitForOrchestrationCompletionAsync(
                gatewayClient, 
                orchestrationResponse.Id, 
                logger, 
                cancellationToken,
                timeoutSeconds,
                pollingIntervalMs);

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("Organization creation was cancelled for: {Name}", request.Name);
            return Results.Problem(
                statusCode: StatusCodes.Status408RequestTimeout,
                title: "Request was cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating organization: {Name}", request.Name);
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error");
        }
    }

    private static async Task<IResult> WaitForOrchestrationCompletionAsync(
        IGatewayClient gatewayClient,
        string instanceId,
        ILogger<Program> logger,
        CancellationToken cancellationToken,
        int timeoutSeconds = 30,
        int pollingIntervalMs = 1000)
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
        
        while (!combinedCts.Token.IsCancellationRequested)
        {
            try
            {
                var statusResult = await gatewayClient.GetAsync<OrchestrationStatus>(
                    $"/api/orchestrators/runtime/webhooks/durabletask/instances/{instanceId}", 
                    combinedCts.Token);
                
                if (statusResult == null)
                {
                    logger.LogWarning("Failed to get orchestration status via Gateway for instance: {InstanceId}", instanceId);
                    await Task.Delay(pollingIntervalMs, combinedCts.Token);
                    continue;
                }

                logger.LogDebug("Orchestration {InstanceId} status: {RuntimeStatus}", instanceId, statusResult.RuntimeStatus);

                switch (statusResult.RuntimeStatus?.ToLowerInvariant())
                {
                    case "completed":
                        logger.LogInformation("Orchestration completed successfully for instance: {InstanceId}", instanceId);
                        
                        // Parse the output from the orchestration
                        if (statusResult.Output != null)
                        {
                            var output = JsonSerializer.Deserialize<CreateOrganizationResult>(statusResult.Output.ToString() ?? "{}");
                            return Results.Ok(output ?? new CreateOrganizationResult 
                            { 
                                Success = true, 
                                Message = "Organization created successfully",
                                OrganizationId = null
                            });
                        }
                        
                        return Results.Ok(new CreateOrganizationResult 
                        { 
                            Success = true, 
                            Message = "Organization created successfully",
                            OrganizationId = null
                        });

                    case "failed":
                    case "terminated":
                        logger.LogError("Orchestration failed or was terminated for instance: {InstanceId}. Output: {Output}", 
                            instanceId, statusResult.Output);
                        
                        var errorMessage = "Organization creation failed";
                        if (statusResult.Output != null)
                        {
                            try
                            {
                                var errorOutput = JsonSerializer.Deserialize<Dictionary<string, object>>(statusResult.Output.ToString() ?? "{}");
                                if (errorOutput?.ContainsKey("message") == true)
                                {
                                    errorMessage = errorOutput["message"]?.ToString() ?? errorMessage;
                                }
                            }
                            catch
                            {
                                // Use default error message if parsing fails
                            }
                        }
                        
                        return Results.BadRequest(new CreateOrganizationResult 
                        { 
                            Success = false, 
                            Message = errorMessage,
                            OrganizationId = null
                        });

                    case "running":
                    case "pending":
                    case "continueasnew":
                        // Continue polling
                        break;

                    default:
                        logger.LogWarning("Unknown orchestration status: {Status} for instance: {InstanceId}", 
                            statusResult.RuntimeStatus, instanceId);
                        break;
                }

                await Task.Delay(pollingIntervalMs, combinedCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                logger.LogWarning("Orchestration wait timeout for instance: {InstanceId}", instanceId);
                return Results.Problem(
                    statusCode: StatusCodes.Status408RequestTimeout,
                    title: "Organization creation timeout",
                    detail: $"The organization creation process did not complete within {timeoutSeconds} seconds. The process may still be running in the background.");
            }
        }

        return Results.Problem(
            statusCode: StatusCodes.Status408RequestTimeout,
            title: "Request cancelled or timeout");
    }
}

// Response models for synchronous operation
public record CreateOrganizationResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public Guid? OrganizationId { get; init; }
}

// Models for Gateway communication (orchestration responses)
public record OrchestrationStatusResponse(
    string Id,
    string StatusQueryGetUri,
    string SendEventPostUri,
    string TerminatePostUri,
    string PurgeHistoryDeleteUri);

public record OrchestrationStatus
{
    public string? RuntimeStatus { get; init; }
    public object? Output { get; init; }
    public DateTime CreatedTime { get; init; }
    public DateTime LastUpdatedTime { get; init; }
}
