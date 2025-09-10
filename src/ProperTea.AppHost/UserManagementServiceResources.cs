using Aspire.Hosting.Azure;
using Projects;

namespace ProperTea.AppHost;

public static class UserManagementServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterOrganizationServiceResources(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<AzureSqlServerResource> sqlServerBuilder,
        string environment)
    {
        var userManagementDb = sqlServerBuilder.AddDatabase("propertea-user-management-db");
        var userManagementMigrations = builder.AddProject<ProperTea_UserManagement_MigrationService>(
                "propertea-user-management-migrations")
            .WithReference(userManagementDb)
            .WaitFor(userManagementDb);
        var userManagementApi = builder
            .AddProject<ProperTea_UserManagement_Api>("user-management-api")
            .WaitFor(userManagementDb)
            .WithReference(userManagementDb)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

        return userManagementApi;
    }
}