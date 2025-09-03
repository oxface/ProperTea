var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("propertea-cosmos").RunAsEmulator(e =>
{
    e.WithLifetime(ContainerLifetime.Persistent);
    e.WithDataVolume();
});

var userManagementDb = cosmos.AddCosmosDatabase("propertea-user-management-db");
var usersContainer = userManagementDb.AddContainer("users", "/id");
var userManagementApi = builder
    .AddProject<Projects.ProperTea_UserManagement_Api>("user-management-api")
    .WithReference(usersContainer);

// var organizationApi = builder.AddProject<Projects.ProperTea_Organization_Api>("organization-api");
//
// var identityApi = builder.AddProject<Projects.ProperTea_Identity_Api>("identity-api");
//
// 

var workflowOrchestrator = builder
    .AddProject<Projects.ProperTea_WorkflowOrchestrator>("workflow-api");

var landlordBff = builder.AddProject<Projects.ProperTea_Landlord_Bff>("landlord-bff");
var gateway = builder.AddProject<Projects.ProperTea_Gateway>("gateway");

landlordBff.WithReference(gateway);

gateway
    // .WithReference(organizationApi)
    // .WithReference(identityApi)
    .WithReference(userManagementApi)
    .WithReference(workflowOrchestrator);

builder.Build().Run();