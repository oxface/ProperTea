using System.Text.Json.Serialization;

using ProperTea.Shared.ServiceDefaults;
using ProperTea.SystemUser.Api.Endpoints;
using ProperTea.SystemUser.Api.Setup;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddGlobalErrorHandling();

builder.Services
    .AddDomainServices()
    .AddDataServices(builder.Configuration)
    .AddInfrastructureServices()
    .AddApplicationServices();

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();

builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
    app.MapScalarApiReference();

app.UseExceptionHandler();
app.UseStatusCodePages();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.MapDefaultEndpoints()
    .MapSystemUserEndpoints();

app.Run();