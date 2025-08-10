using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class DeleteSystemUserActivity : WorkflowActivity<string, object>
{
    public override async Task<object> RunAsync(WorkflowActivityContext context, string userId)
    {
        var daprClient = new DaprClientBuilder().Build();
        await daprClient.InvokeMethodAsync(
            HttpMethod.Delete, "propertea-systemuser-api", "system-user", userId);
        return null!;
    }
}