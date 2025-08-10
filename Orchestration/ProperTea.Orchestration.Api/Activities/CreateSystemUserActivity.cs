using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class CreateSystemUserActivity : WorkflowActivity<CreateSystemUserRequest, CreateSystemUserResponse>
{
    public override async Task<CreateSystemUserResponse> RunAsync(WorkflowActivityContext context, CreateSystemUserRequest input)
    {
        var daprClient = new DaprClientBuilder().Build();
        var response = await daprClient.InvokeMethodAsync<CreateSystemUserRequest, CreateSystemUserResponse>(
            "propertea-systemuser-api", "system-user", input);
        return response;
    }
}

public record CreateSystemUserRequest(string OrganizationId, string Email, string DisplayName, string Role);

public record CreateSystemUserResponse(string UserId);