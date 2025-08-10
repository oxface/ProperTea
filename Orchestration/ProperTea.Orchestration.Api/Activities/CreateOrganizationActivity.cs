using Dapr.Workflow;
using System.Threading.Tasks;
using ProperTea.Orchestration.Api.Workflows;
using Dapr.Client;

namespace ProperTea.Orchestration.Api.Activities
{
    public class CreateOrganizationActivity(DaprClient daprClient) : WorkflowActivity<CreateOrganizationRequest, CreateOrganizationResponse>
    {
        public override async Task<CreateOrganizationResponse> RunAsync(WorkflowActivityContext context, CreateOrganizationRequest input)
        {
            var response = await daprClient.InvokeMethodAsync<CreateOrganizationRequest, CreateOrganizationResponse>(
                "propertea-organization-api", "organization", input);
            return response;
        }
    }
          
    public record CreateOrganizationRequest(string Name);
    public record CreateOrganizationResponse(string OrganizationId);
}

