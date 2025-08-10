using ProperTea.Server.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var azureSql = builder.AddAzureSqlServer("propertea-sql")
    .RunAsContainer(e =>
    {
        e.WithDataVolume("propertea-sql-data");
        e.WithLifetime(ContainerLifetime.Persistent);
    });

var organizationApi = builder.RegisterOrganizationServiceResources(azureSql);
var systemUserApi = builder.RegisterSystemUserServiceResources(azureSql, organizationApi);
var companyApi = builder.RegisterCompanyServiceResources(azureSql, organizationApi);

var gateway = builder.AddProject<Projects.ProperTea_LandlordPortal_Gateway>("landlord-portal-gateway")
    .WithReference(organizationApi)
    .WaitFor(organizationApi)
    .WithReference(systemUserApi)
    .WaitFor(systemUserApi)
    .WithReference(companyApi)
    .WaitFor(companyApi)
    .WithExternalHttpEndpoints()
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

builder.Build().Run();