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
        var db = sqlServerBuilder.AddDatabase("propertea-company-db");
        var migrations = builder.AddProject<ProperTea_Company_MigrationService>(
                "company-migrations")
            .WithReference(db)
            .WaitFor(db);

        var apiSidecar = new DaprSidecarOptions
        {
            AppId = "company-api-sidecar",
            AppPort = 5000,
            DaprHttpPort = 5010,
            DaprGrpcPort = 5011,
            MetricsPort = 5012
        };
        var api = builder
            .AddProject<ProperTea_Company_Api>("company-api")
            .WithReference(db)
            .WithReference(migrations)
            .WaitForCompletion(migrations)
            .WaitFor(db)
            .WithReference(systemOwnerApiBuilder)
            .WaitFor(systemOwnerApiBuilder)
            .WithDaprSidecar(apiSidecar)
            .WithOtlpExporter()
            .WithHttpHealthCheck("/health")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

        return api;
    }
}