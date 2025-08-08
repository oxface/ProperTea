using ProperTea.Server.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var azureSql = builder.AddAzureSqlServer("propertea-sql")
    .RunAsContainer(e =>
    {
        e.WithDataVolume("propertea-sql-data");
        e.WithLifetime(ContainerLifetime.Persistent);
    });

var systemOwnerApi = builder.RegisterSystemOwnerServiceResources(azureSql);
var companyApi = builder.RegisterCompanyServiceResources(azureSql, systemOwnerApi);

builder.Build().Run();