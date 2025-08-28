using Microsoft.Azure.Cosmos;
using ProperTea.Infrastructure.Extensions;
using ProperTea.Identity.Api.Application.Handlers;
using ProperTea.Identity.Api.Domain.Identities;
using ProperTea.Identity.Api.Endpoints;
using ProperTea.Identity.Api.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Global Error Handling - using shared infrastructure
builder.Services.AddGlobalErrorHandling("ProperTea.Identity.Api");

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
    return cosmosClient.GetContainer("ProperTea", "Identities");
});

// Add custom CQRS
builder.Services.AddCustomCqrs();

// Add command handlers
builder.Services.AddCommandHandlers(typeof(CreateIdentityCommandHandler));

// Add repositories
builder.Services.AddScoped<IUserIdentityRepository, CosmosUserIdentityRepository>();

var app = builder.Build();

// Global Error Handling - using shared infrastructure
app.UseGlobalErrorHandling("ProperTea.Identity.Api");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapIdentityEndpoints();

app.MapDefaultEndpoints();

app.Run();
