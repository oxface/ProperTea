using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Polly;
using Polly.Extensions.Http;
using ProperTea.Infrastructure.Shared.Extensions;
using ProperTea.ServiceDefaults;
using ProperTea.WorkflowOrchestrator.Endpoints.Organization;
using ProperTea.WorkflowOrchestrator.Endpoints.UserIdentity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.WorkflowOrchestrator");

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    });
builder.Services.AddAuthorization();

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

builder.Services.AddHttpClient("gateway", client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "ProperTea-WorkflowOrchestrator/1.0");
        client.BaseAddress = new Uri("https://gateway");
        client.Timeout = TimeSpan.FromSeconds(300);
    })
    .AddServiceDiscovery()
    .AddPolicyHandler(retryPolicy);

var app = builder.Build();

app.UseGlobalErrorHandling("ProperTea.WorkflowOrchestrator");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapUserWorkflowEndpoints();
app.MapOrganizationWorkflowEndpoints();

app.MapDefaultEndpoints();

app.Run();