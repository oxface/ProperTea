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

builder.Build().Run();