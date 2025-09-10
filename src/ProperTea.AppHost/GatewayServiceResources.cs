using Projects;

namespace ProperTea.AppHost;

public static class GatewayServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterGatewayServiceResources(
        this IDistributedApplicationBuilder builder,
        string environment)
    {
        var gateway = builder.AddProject<ProperTea_Gateway>("gateway")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

        return gateway;
    }
}