using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.Server.AppHost;

public static class SystemOwnerServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterSystemOwnerServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder)
    {
        // Ports 5000-5099.
        // 10 ports per service.
        var db = sqlServerBuilder.AddDatabase("propertea-systemowner-db");
        var migrations = builder.AddProject<ProperTea_SystemOwner_MigrationService>(
                "systemowner-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "systemowner-api-sidecar",
            AppPort = 5000,
            DaprHttpPort = 5001,
            DaprGrpcPort = 5002,
            MetricsPort = 5003
        };
        var api = builder
            .AddProject<ProperTea_SystemOwner_Api>("systemowner-api")
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