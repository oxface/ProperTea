using Dapr.Workflow;
using System.Threading.Tasks;
using ProperTea.Orchestration.Api.Workflows;
using Dapr.Client;

namespace ProperTea.Orchestration.Api.Activities
{
    public class CreateUserIdentityActivity : WorkflowActivity<CreateUserIdentityRequest, CreateUserIdentityResponse>
    {
        public override async Task<CreateUserIdentityResponse> RunAsync(WorkflowActivityContext context, CreateUserIdentityRequest input)
        {
            var daprClient = new DaprClientBuilder().Build();
            var response = await daprClient.InvokeMethodAsync<CreateUserIdentityRequest, CreateUserIdentityResponse>(
                "propertea-identity-api", "identity", input);
            return response;
        }
    }
    
    public record CreateUserIdentityRequest(string UserId, string Email, string Password);
    public record CreateUserIdentityResponse(bool Success);
}
