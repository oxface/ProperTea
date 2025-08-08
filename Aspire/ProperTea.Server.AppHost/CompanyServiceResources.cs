using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.Server.AppHost;

public static class CompanyServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterCompanyServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder,
        IResourceBuilder<ProjectResource> systemOwnerApiBuilder)
    {
        // Ports 5200-5299.
        // 10 ports per service.
        var db = sqlServerBuilder.AddDatabase("propertea-company-db");
        var migrations = builder.AddProject<ProperTea_Company_MigrationService>(
                "company-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "company-api-sidecar",
            AppPort = 5200,
            DaprHttpPort = 5201,
            DaprGrpcPort = 5202,
            MetricsPort = 5003
        };
        var api = builder
            .AddProject<ProperTea_Company_Api>("company-api")
            .WithReference(db)
            .WaitFor(db)
            .WithReference(systemOwnerApiBuilder)
            .WaitFor(systemOwnerApiBuilder)
            .WithReference(migrations)
            .WaitForCompletion(migrations)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

        return api;
    }
}