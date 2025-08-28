using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using ProperTea.Contracts.DTOs.Organization;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using ProperTea.WorkflowOrchestrator.Activities;

namespace ProperTea.WorkflowOrchestrator.Orchestrators;

public class OrganizationCreationOrchestrator
{
    private readonly ILogger<OrganizationCreationOrchestrator> _logger;

    public OrganizationCreationOrchestrator(ILogger<OrganizationCreationOrchestrator> logger)
    {
        _logger = logger;
    }

    [Function("CreateOrganizationOrchestrator")]
    public async Task<CreateOrganizationResponse> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var request = context.GetInput<CreateOrganizationRequest>()!;
        
        Guid? userId = null;
        Guid? organizationId = null;
        var userCreated = false;
        var identityCreated = false;
        var organizationCreated = false;
        var userAddedToOrg = false;
        
        try
        {
            _logger.LogInformation("Starting organization creation orchestration for {Name}", request.Name);

            // Step 1: Check if user already exists
            var userExists = await context.CallActivityAsync<bool>(
                "CheckUserExistsActivity", 
                request.AdminEmail);

            if (!userExists)
            {
                // Step 2a: Create new user
                _logger.LogInformation("Creating new user for email {Email}", request.AdminEmail);
                
                userId = await context.CallActivityAsync<Guid>(
                    "CreateUserActivity", 
                    new CreateUserRequest(request.AdminEmail, request.AdminFullName));
                userCreated = true;

                // Step 2b: Create identity for new user
                await context.CallActivityAsync(
                    "CreateIdentityActivity", 
                    new CreateIdentityRequest(userId.Value, request.AdminEmail, request.AdminPassword));
                identityCreated = true;
            }
            else
            {
                // Step 2c: Get existing user
                _logger.LogInformation("Using existing user for email {Email}", request.AdminEmail);
                
                userId = await context.CallActivityAsync<Guid>(
                    "GetUserByEmailActivity", 
                    request.AdminEmail);
            }

            // Step 3: Create organization
            organizationId = await context.CallActivityAsync<Guid>(
                "CreateOrganizationActivity",
                new CreateOrganizationActivityRequest(request.Name, request.Description, userId.Value));
            organizationCreated = true;

            // Step 4: Add user as admin to organization
            await context.CallActivityAsync(
                "AddUserToOrganizationActivity",
                new AddUserToOrganizationRequest(userId.Value, organizationId.Value, "Admin"));
            userAddedToOrg = true;

            // Step 5: Activate organization
            await context.CallActivityAsync(
                "ActivateOrganizationActivity",
                organizationId.Value);

            _logger.LogInformation("Organization creation completed for {Name} with ID {OrganizationId}", 
                request.Name, organizationId);

            return new CreateOrganizationResponse(
                organizationId.Value, 
                request.Name, 
                userId.Value, 
                request.AdminEmail, 
                userCreated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Organization creation failed for {Name}, starting compensation", request.Name);
            
            // Compensate in reverse order
            if (userAddedToOrg && userId.HasValue && organizationId.HasValue)
            {
                await context.CallActivityAsync("CompensateAddUserToOrganizationActivity",
                    new RemoveUserFromOrganizationRequest(userId.Value, organizationId.Value));
            }

            if (organizationCreated && organizationId.HasValue)
            {
                await context.CallActivityAsync("CompensateCreateOrganizationActivity", organizationId.Value);
            }

            if (identityCreated && userId.HasValue)
            {
                await context.CallActivityAsync("CompensateCreateIdentityActivity", userId.Value);
            }

            if (userCreated && userId.HasValue)
            {
                await context.CallActivityAsync("CompensateCreateUserActivity", userId.Value);
            }

            throw;
        }
    }

    [Function("CreateOrganizationStarter")]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("CreateOrganizationStarter");

        var request = await req.ReadFromJsonAsync<CreateOrganizationRequest>();
        if (request == null)
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid request body");
            return badResponse;
        }

        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            "CreateOrganizationOrchestrator", request);

        logger.LogInformation("Started orchestration with ID = '{instanceId}'", instanceId);

        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}
