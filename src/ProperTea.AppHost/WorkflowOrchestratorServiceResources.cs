using Aspire.Hosting.Azure;
using Projects;

namespace ProperTea.AppHost;

public static class WorkflowOrchestratorServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterWorkflowOrchestratorServiceResources(
        this IDistributedApplicationBuilder builder,
        string environment)
    {
        var workflowOrchestrator = builder
            .AddProject<ProperTea_WorkflowOrchestrator>("workflow-api")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

        return workflowOrchestrator;
    }
}