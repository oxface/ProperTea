using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class DeleteSystemUserActivity(DaprClient daprClient) : WorkflowActivity<string, object>
{
    public override async Task<object> RunAsync(WorkflowActivityContext context, string userId)
    {
        await daprClient.InvokeMethodAsync(
            HttpMethod.Delete, "propertea-systemuser-api", $"system-user/{userId}");
        return null!;
    }
}