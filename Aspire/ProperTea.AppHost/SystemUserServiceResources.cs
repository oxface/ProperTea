using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class SystemUserServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterSystemUserServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder,
        IResourceBuilder<ProjectResource> organizationApiBuilder)
    {
        // Ports 5050-5099.
        // 10 ports per service.
        const int apiPort = 5050;
        var db = sqlServerBuilder.AddDatabase("propertea-systemuser-db");
        var migrations = builder.AddProject<ProperTea_SystemUser_MigrationService>(
                "propertea-systemuser-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-systemuser-api",
            AppPort = apiPort,
            DaprHttpPort = 5052,
            DaprGrpcPort = 5053,
            MetricsPort = 5054
        };
        var api = builder
            .AddProject<ProperTea_SystemUser_Api>("propertea-systemuser-api")
            .WithHttpEndpoint(apiPort)
            .WithHttpsEndpoint(apiPort + 1)
            .WithReference(db)
            .WaitFor(db)
            .WithReference(organizationApiBuilder)
            .WaitFor(organizationApiBuilder)
            .WithReference(migrations)
            .WaitForCompletion(migrations)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

        return api;
    }
}