using Dapr.Client;
using Dapr.Workflow;

using ProperTea.Orchestration.Api.Activities;
using ProperTea.Orchestration.Api.Workflows;
using ProperTea.Shared.ServiceDefaults;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddDaprClient();
builder.Services.AddDaprWorkflow(options =>
{
    options.RegisterWorkflow<RegisterOrganizationWorkflow>();
    options.RegisterActivity<CreateOrganizationActivity>();
    options.RegisterActivity<DeleteOrganizationActivity>();
    options.RegisterActivity<CreateSystemUserActivity>();
    options.RegisterActivity<DeleteSystemUserActivity>();
    options.RegisterActivity<CreateUserIdentityActivity>();
});

var app = builder.Build();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.MapPost("/register-organization", 
    async (RegisterOrganizationWorkflowInput input, DaprWorkflowClient workflowClient) =>
    {
        var instanceId = await workflowClient.ScheduleNewWorkflowAsync(
            nameof(RegisterOrganizationWorkflow),
            input: input);
        var state = await workflowClient.WaitForWorkflowCompletionAsync(instanceId);
        if (state.RuntimeStatus != WorkflowRuntimeStatus.Completed)
            return Results.BadRequest(state.FailureDetails?.ErrorMessage);
        
        var result = state.ReadOutputAs<RegisterOrganizationWorkflowResult>();
        return !result!.Success
            ? Results.BadRequest(state.FailureDetails?.ErrorMessage) 
            : Results.Created($"/organization/{result.OrganizationId}", new { result.OrganizationId, result.AdminUserId });
    });

app.Run();