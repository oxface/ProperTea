using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Extensions;
using ProperTea.UserManagement.Api.Application.Handlers;
using ProperTea.UserManagement.Api.Domain.Users;
using ProperTea.UserManagement.Api.Endpoints;
using ProperTea.UserManagement.Api.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Global Error Handling - using shared infrastructure
builder.Services.AddGlobalErrorHandling("ProperTea.UserManagement.Api");

// Add Azure Cosmos DB
builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb") ?? 
        "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b4mXFQ2K/PlhKQYABqQjvOJV1BcOPw==";
    return new CosmosClient(connectionString);
});

builder.Services.AddScoped<Container>(serviceProvider =>
{
    var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
    return cosmosClient.GetContainer("ProperTea", "Users");
});

// Add custom CQRS
builder.Services.AddCustomCqrs();

// Add command and query handlers
builder.Services.AddCommandHandlers(typeof(CreateSystemUserCommandHandler));
builder.Services.AddQueryHandlers(typeof(GetUserByIdQueryHandler));

// Add repositories
builder.Services.AddScoped<ISystemUserRepository, CosmosSystemUserRepository>();

var app = builder.Build();

// Global Error Handling - using shared infrastructure
app.UseGlobalErrorHandling("ProperTea.UserManagement.Api");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapUserEndpoints();

app.MapDefaultEndpoints();

app.Run();
