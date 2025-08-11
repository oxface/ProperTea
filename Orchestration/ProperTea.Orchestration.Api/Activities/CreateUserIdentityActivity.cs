using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class CreateUserIdentityActivity(DaprClient daprClient) : WorkflowActivity<CreateUserIdentityRequest, Guid>
{
    public override async Task<Guid> RunAsync(WorkflowActivityContext context, CreateUserIdentityRequest input)
    {
        var response = await daprClient.InvokeMethodAsync<CreateUserIdentityRequest, Guid>(
            "propertea-identity-api", "user-identity", input);
        return response;
    }
}

public record CreateUserIdentityRequest(Guid SystemUserId, string Email, string Password);

public record CreateUserIdentityResponse(bool Success);