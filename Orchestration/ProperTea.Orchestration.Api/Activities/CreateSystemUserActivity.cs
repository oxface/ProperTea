using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class CreateSystemUserActivity(DaprClient daprClient) : WorkflowActivity<CreateSystemUserRequest, Guid>
{
    public override async Task<Guid> RunAsync(WorkflowActivityContext context, CreateSystemUserRequest input)
    {
        var response = await daprClient.InvokeMethodAsync<CreateSystemUserRequest, Guid>(
            "propertea-systemuser-api", "system-user", input);
        return response;
    }
}

public record CreateSystemUserRequest(string Name);

public record CreateSystemUserResponse(string UserId);