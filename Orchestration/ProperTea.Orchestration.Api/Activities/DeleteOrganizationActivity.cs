using Dapr.Workflow;
using System.Threading.Tasks;
using Dapr.Client;

namespace ProperTea.Orchestration.Api.Activities
{
    public class DeleteOrganizationActivity : WorkflowActivity<string, object>
    {
        public override async Task<object> RunAsync(WorkflowActivityContext context, string organizationId)
        {
            var daprClient = new DaprClientBuilder().Build();
            await daprClient.InvokeMethodAsync(
                HttpMethod.Delete, "propertea-organization-api", "organization", organizationId);
            return null!;
        }
    }
}

