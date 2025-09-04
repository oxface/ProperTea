using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var environment = "Development";

var cosmos = builder.AddAzureCosmosDB("propertea-cosmos").RunAsEmulator(e =>
{
    e.WithLifetime(ContainerLifetime.Persistent);
    e.WithDataVolume();
});

var userManagementDb = cosmos.AddCosmosDatabase("propertea-user-management-db");
var usersContainer = userManagementDb.AddContainer("users", "/id");
var userManagementApi = builder
    .AddProject<ProperTea_UserManagement_Api>("user-management-api")
    .WaitFor(usersContainer)
    .WithReference(usersContainer)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

var workflowOrchestrator = builder
    .AddProject<ProperTea_WorkflowOrchestrator>("workflow-api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

var landlordBff = builder.AddProject<ProperTea_Landlord_Bff>("landlord-bff")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);
var gateway = builder.AddProject<ProperTea_Gateway>("gateway")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

userManagementApi.WithReference(gateway);
landlordBff.WithReference(gateway);

gateway
    .WithReference(userManagementApi)
    .WithReference(workflowOrchestrator);

builder.Build().Run();