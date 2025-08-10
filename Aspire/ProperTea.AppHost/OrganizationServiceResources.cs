using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class OrganizationServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterOrganizationServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder)
    {
        // Ports 5000-5049.
        // 10 ports per API.
        const int apiPort = 5000;
        var db = sqlServerBuilder.AddDatabase("propertea-organization-db");
        var migrations = builder.AddProject<ProperTea_Organization_MigrationService>(
                "propertea-organization-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-organization-api",
            AppPort = apiPort,
            DaprHttpPort = 5002,
            DaprGrpcPort = 5003,
            MetricsPort = 5004
        };
        var api = builder
            .AddProject<ProperTea_Organization_Api>("propertea-organization-api")
            .WithHttpEndpoint(apiPort)
            .WithHttpsEndpoint(apiPort + 1)
            .WithReference(db)
            .WaitFor(db)
            .WithReference(migrations)
            .WaitForCompletion(migrations)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

        return api;
    }
}