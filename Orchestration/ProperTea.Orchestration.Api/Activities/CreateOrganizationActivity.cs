using Dapr.Client;
using Dapr.Workflow;

namespace ProperTea.Orchestration.Api.Activities;

public class CreateOrganizationActivity(DaprClient daprClient) : WorkflowActivity<CreateOrganizationRequest, Guid>
{
    public override async Task<Guid> RunAsync(WorkflowActivityContext context, CreateOrganizationRequest input)
    {
        var response = await daprClient.InvokeMethodAsync<CreateOrganizationRequest, Guid>(
            "propertea-organization-api", "organization", input);
        return response;
    }
}

public record CreateOrganizationRequest(string Name);

public record CreateOrganizationResponse(string OrganizationId);