using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class GatewayResources
{
    public static IResourceBuilder<ProjectResource> RegisterLandlordGatewayResources(
        this IDistributedApplicationBuilder builder,
        IEnumerable<IResourceBuilder<ProjectResource>> waitForProjects)
    {
        // Ports 5950-5999.
        // 10 ports per service.
        const int apiPort = 5950;
        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-landlord-portal-gateway",
            AppPort = apiPort,
            DaprHttpPort = 5952,
            DaprGrpcPort = 5953,
            MetricsPort = 5954
        };
        var gateway = builder
            .AddProject<ProperTea_LandlordPortal_Gateway>("propertea-landlord-portal-gateway")
            .WithHttpEndpoint(apiPort)
            .WithHttpsEndpoint(apiPort + 1)
            .WithExternalHttpEndpoints()
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");
        waitForProjects.ToList().ForEach(p =>
        {
            gateway.WithReference(p);
            gateway.WaitFor(p);
        });
        return gateway;
    }
}