using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

var builder = FunctionsApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration.AddEnvironmentVariables();

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

var services = builder.Services;
        
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

services.AddHttpClient("Gateway", client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "ProperTea-WorkflowOrchestrator/1.0");
        client.BaseAddress = new Uri("https://gateway");
    })
    .AddServiceDiscovery()
    .AddPolicyHandler(retryPolicy);
        
services.AddScoped<HttpClient>(serviceProvider =>
    serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Gateway"));

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
