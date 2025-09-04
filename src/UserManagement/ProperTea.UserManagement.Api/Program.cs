using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using ProperTea.Cqrs;
using ProperTea.ServiceDefaults;
using ProperTea.Shared.Infrastructure.Extensions;
using ProperTea.UserManagement.Api.Application.Handlers;
using ProperTea.UserManagement.Api.Domain.Users;
using ProperTea.UserManagement.Api.Endpoints;
using ProperTea.UserManagement.Api.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.UserManagement.Api");

builder.Services.AddScoped<CosmosClient>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("propertea-cosmos")
                           ?? throw new InvalidOperationException("CosmosDb connection string is not configured.");
    return new CosmosClient(connectionString);
});

builder.Services.AddProperCqrs();
builder.Services.AddProperCqrsCommandHandlers(typeof(CreateSystemUserCommandHandler).Assembly);
builder.Services.AddProperCqrsQueryHandlers(typeof(GetUserByIdQueryHandler).Assembly);

builder.Services.AddScoped<ISystemUserRepository, CosmosSystemUserRepository>(f =>
    new CosmosSystemUserRepository(f.GetRequiredService<CosmosClient>(),
        "propertea-user-management-db",
        "users"));


var app = builder.Build();

app.UseGlobalErrorHandling("ProperTea.UserManagement.Api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();

app.MapDefaultEndpoints();

app.Run();