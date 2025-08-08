using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.Server.AppHost;

public static class OrganizationServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterOrganizationServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder)
    {
        // Ports 5000-5099.
        // 10 ports per service.
        var db = sqlServerBuilder.AddDatabase("propertea-organization-db");
        var migrations = builder.AddProject<ProperTea_Organization_MigrationService>(
                "organization-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "organization-api-sidecar",
            AppPort = 5000,
            DaprHttpPort = 5001,
            DaprGrpcPort = 5002,
            MetricsPort = 5003
        };
        var api = builder
            .AddProject<ProperTea_Organization_Api>("organization-api")
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