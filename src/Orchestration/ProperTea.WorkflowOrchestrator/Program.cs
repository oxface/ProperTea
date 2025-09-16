using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using ProperTea.ServiceDefaults;
using ProperTea.Shared.Infrastructure.Extensions;
using ProperTea.WorkflowOrchestrator.Endpoints.UserIdentity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults(
    false);

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

builder.Services.AddHttpClient("gateway", client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "ProperTea-WorkflowOrchestrator/1.0");
        client.BaseAddress = new Uri("https://gateway");
    })
    .AddServiceDiscovery()
    .AddStandardResilienceHandler(o =>
    {
        o.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(240);
        o.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(240);
        o.AttemptTimeout.Timeout = TimeSpan.FromSeconds(120);
        o.Retry.MaxDelay = TimeSpan.FromSeconds(120);
        o.Retry.BackoffType = DelayBackoffType.Exponential;
        o.Retry.DisableForUnsafeHttpMethods();
    });
;

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

app.MapDefaultEndpoints();

app.Run();