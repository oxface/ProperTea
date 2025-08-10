using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class IdentityServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterIdentityServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder)
    {
        // Ports 5150-5199.
        // 10 ports per service.
        const int apiPort = 5150;
        var db = sqlServerBuilder.AddDatabase("propertea-identity-db");
        var migrations = builder.AddProject<ProperTea_Identity_MigrationService>(
                "propertea-identity-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-identity-api",
            AppPort = apiPort,
            DaprHttpPort = 5152,
            DaprGrpcPort = 5153,
            MetricsPort = 5154
        };
        var api = builder
            .AddProject<ProperTea_SystemUser_Api>("propertea-identity-api")
            .WithHttpEndpoint(port: apiPort)
            .WithHttpsEndpoint(port: apiPort + 1)
            .WithExternalHttpEndpoints()
            .WithReference(db)
            .WaitFor(db)
            .WithReference(migrations)
            .WaitFor(migrations)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

        return api;
    }
}