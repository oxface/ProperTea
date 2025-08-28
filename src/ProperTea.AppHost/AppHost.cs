using Azure.Provisioning.Storage;

var builder = DistributedApplication.CreateBuilder(args);

// Add Azure Storage for Functions
var storage = builder.AddAzureStorage("storage").RunAsEmulator();

var gateway = builder.AddProject<Projects.ProperTea_Gateway>("gateway");

// var organizationApi = builder.AddProject<Projects.ProperTea_Organization_Api>("organization-api");
//
// var identityApi = builder.AddProject<Projects.ProperTea_Identity_Api>("identity-api");
//
// var userManagementApi = builder.AddProject<Projects.ProperTea_UserManagement_Api>("user-management-api");

var workflowOrchestrator = builder
    .AddAzureFunctionsProject<Projects.ProperTea_WorkflowOrchestrator>("workflow-orchestrator")
    .WithHostStorage(storage);

var landlordBff = builder.AddProject<Projects.ProperTea_Landlord_Bff>("landlord-bff")
    .WithReference(gateway);

gateway
   // .WithReference(organizationApi)
   // .WithReference(identityApi)
   // .WithReference(userManagementApi)
   .WithReference(workflowOrchestrator);

workflowOrchestrator.WithReference(gateway);

builder.Build().Run();