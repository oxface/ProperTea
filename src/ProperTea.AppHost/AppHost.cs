using Projects;
using ProperTea.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var environment = "Development";

var azureSql = builder.AddAzureSqlServer("propertea-azuresql").RunAsContainer(c =>
{
    c.WithLifetime(ContainerLifetime.Persistent);
    c.WithDataVolume();
});

var userManagementApi = builder.RegisterOrganizationServiceResources(azureSql, environment);

var workflowOrchestrator = builder.RegisterWorkflowOrchestratorServiceResources(environment);

var landlordBff = builder.RegisterLandlordBffServiceResources(environment);
var gateway = builder.RegisterGatewayServiceResources(environment);

userManagementApi.WithReference(gateway);
landlordBff.WithReference(gateway);
workflowOrchestrator.WithReference(gateway);

gateway
    .WithReference(userManagementApi)
    .WithReference(workflowOrchestrator);

builder.Build().Run();