using Aspire.Hosting.Azure;

using CommunityToolkit.Aspire.Hosting.Dapr;

using Projects;

namespace ProperTea.AppHost;

public static class CompanyServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterCompanyServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder,
        IResourceBuilder<ProjectResource> organizationApiBuilder)
    {
        // Ports 5100-5149.
        // 10 ports per service.
        const int apiPort = 5100;
        var db = sqlServerBuilder.AddDatabase("propertea-company-db");
        var migrations = builder.AddProject<ProperTea_Company_MigrationService>(
                "propertea-company-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "propertea-company-api",
            AppPort = apiPort,
            DaprHttpPort = 5102,
            DaprGrpcPort = 5103,
            MetricsPort = 5104
        };
        var api = builder
            .AddProject<ProperTea_Company_Api>("propertea-company-api")
            .WithHttpEndpoint(port: apiPort)
            .WithHttpsEndpoint(port: apiPort + 1)
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