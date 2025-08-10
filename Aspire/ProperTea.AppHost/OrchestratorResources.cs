using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class OrchestratorServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterOrchestratorServiceResources(
        this IDistributedApplicationBuilder builder,
        IEnumerable<IResourceBuilder<ProjectResource>> waitForProjects)
    {
        // Ports 5900-5949.
        // 10 ports per service.
        const int apiPort = 5900;
        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-orchestration-api",
            AppPort = apiPort,
            DaprHttpPort = 5902,
            DaprGrpcPort = 5903,
            MetricsPort = 5904,
            
        };
        var api = builder
            .AddProject<ProperTea_Orchestration_Api>("propertea-orchestration-api")
            .WithHttpEndpoint(port: apiPort)
            .WithHttpsEndpoint(port: apiPort + 1)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");
        
        waitForProjects.ToList().ForEach(p =>
        {
            api.WithReference(p);
            api.WaitFor(p);
        });

        return api;
    }
}