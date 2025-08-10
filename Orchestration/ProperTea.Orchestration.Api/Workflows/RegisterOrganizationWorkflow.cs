using Dapr.Workflow;

using ProperTea.Orchestration.Api.Activities;

namespace ProperTea.Orchestration.Api.Workflows;

public class RegisterOrganizationWorkflow : Workflow<RegisterOrganizationWorkflowInput, RegisterOrganizationWorkflowResult>
{
    public override async Task<RegisterOrganizationWorkflowResult> RunAsync(WorkflowContext context, RegisterOrganizationWorkflowInput input)
    {
        var logger = context.CreateReplaySafeLogger<RegisterOrganizationWorkflow>();

        CreateOrganizationResponse orgResponse;
        try
        {
            var orgRequest = new CreateOrganizationRequest(input.OrganizationName);
            orgResponse = await context.CallActivityAsync<CreateOrganizationResponse>(
                nameof(CreateOrganizationActivity),
                orgRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create organization.");
            return new RegisterOrganizationWorkflowResult(
                false,
                null,
                null,
                "Failed to create organization.");
        }

        var organizationId = orgResponse.OrganizationId;

        var userRequest = new CreateSystemUserRequest(organizationId, input.AdminEmail, input.AdminDisplayName, "Admin");
        CreateSystemUserResponse userResponse;
        try
        {
            userResponse = await context.CallActivityAsync<CreateSystemUserResponse>(
                nameof(CreateSystemUserActivity),
                userRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create system user.");
            try
            {
                await context.CallActivityAsync(nameof(DeleteOrganizationActivity), organizationId);
            }
            catch (Exception deleteOrgEx)
            {
                if (!deleteOrgEx.Message.Contains("404"))
                    logger.LogError(deleteOrgEx, "Failed to delete organization during compensation.");
            }

            return new RegisterOrganizationWorkflowResult(
                false,
                null,
                null,
                "Failed to create system user.");
        }

        var userId = userResponse.UserId;
        var identityRequest = new CreateUserIdentityRequest(userId, input.AdminEmail, input.AdminPassword);
        try
        {
            await context.CallActivityAsync<CreateUserIdentityResponse>(
                nameof(CreateUserIdentityActivity), identityRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create user identity.");
            try
            {
                await context.CallActivityAsync(nameof(DeleteOrganizationActivity), organizationId);
            }
            catch (Exception deleteOrgEx)
            {
                if (!deleteOrgEx.Message.Contains("404"))
                    logger.LogError(deleteOrgEx, "Failed to delete organization during compensation.");
            }

            try
            {
                await context.CallActivityAsync(nameof(DeleteSystemUserActivity), userId);
            }
            catch (Exception deleteUserEx)
            {
                if (!deleteUserEx.Message.Contains("404"))
                    logger.LogError(deleteUserEx, "Failed to delete system user during compensation.");
            }

            return new RegisterOrganizationWorkflowResult(false,
                null,
                null,
                "Failed to create user identity.");
        }

        return new RegisterOrganizationWorkflowResult(true, organizationId, userId, null);
    }
}

public record RegisterOrganizationWorkflowInput(string OrganizationName, string AdminEmail, string AdminDisplayName, string AdminPassword);

public record RegisterOrganizationWorkflowResult(bool Success, string? OrganizationId, string? AdminUserId, string? ErrorMessage);