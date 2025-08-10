using ProperTea.AppHost;

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
var identityApi = builder.RegisterIdentityServiceResources(azureSql);

var orchestrationApi = builder.RegisterOrchestratorServiceResources(
[
    organizationApi, systemUserApi, companyApi
]);
var landlordGateway = builder.RegisterLandlordGatewayResources(
[
    organizationApi,
    systemUserApi,
    companyApi,
    orchestrationApi
]);


builder.Build().Run();