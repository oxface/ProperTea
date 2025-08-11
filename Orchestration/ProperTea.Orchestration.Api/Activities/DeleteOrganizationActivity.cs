using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class DeleteOrganizationActivity(DaprClient daprClient) : WorkflowActivity<string, object>
{
    public override async Task<object> RunAsync(WorkflowActivityContext context, string organizationId)
    {
        await daprClient.InvokeMethodAsync(
            HttpMethod.Delete, "propertea-organization-api", $"organization/{organizationId}");
        return null!;
    }
}