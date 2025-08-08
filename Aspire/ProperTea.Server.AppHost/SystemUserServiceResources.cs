using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.Server.AppHost;

public static class SystemUserServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterSystemUserServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder,
        IResourceBuilder<ProjectResource> organizationApiBuilder)
    {
        // Ports 5100-5199.
        // 10 ports per service.
        var db = sqlServerBuilder.AddDatabase("propertea-systemuser-db");
        var migrations = builder.AddProject<ProperTea_SystemUser_MigrationService>(
                "systemuser-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "systemuser-api-sidecar",
            AppPort = 5100,
            DaprHttpPort = 5101,
            DaprGrpcPort = 5102,
            MetricsPort = 5103
        };
        var api = builder
            .AddProject<ProperTea_SystemUser_Api>("systemuser-api")
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