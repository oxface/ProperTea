using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Shared.Extensions;
using ProperTea.UserManagement.Api.Application.Handlers;
using ProperTea.UserManagement.Api.Domain.Users;
using ProperTea.UserManagement.Api.Endpoints;
using ProperTea.UserManagement.Api.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.UserManagement.Api");

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("propertea-cosmos")
        ?? throw new InvalidOperationException("CosmosDb connection string is not configured.");
    return new CosmosClient(connectionString);
});

builder.Services.AddScoped<Container>(serviceProvider =>
{
    var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
    return cosmosClient.GetContainer("propertea-user-management-db", "users");
});

builder.Services.AddProperCqrs();

builder.Services.AddCommandHandlers(typeof(CreateSystemUserCommandHandler));
builder.Services.AddQueryHandlers(typeof(GetUserByIdQueryHandler));

builder.Services.AddScoped<ISystemUserRepository, CosmosSystemUserRepository>(f => 
    new CosmosSystemUserRepository(f.GetRequiredService<CosmosClient>(),
        "propertea-user-management-db",
        "users",
        "/id"));

var app = builder.Build();

app.UseGlobalErrorHandling("ProperTea.UserManagement.Api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.MapDefaultEndpoints();

app.Run();
