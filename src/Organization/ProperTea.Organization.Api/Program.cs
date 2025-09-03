using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProperTea.Infrastructure.Shared.Extensions;
using ProperTea.Organization.Api.Application.Handlers;
using ProperTea.Organization.Api.Domain.Organizations;
using ProperTea.Organization.Api.Endpoints;
using ProperTea.Organization.Api.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    });

builder.Services.AddAuthorization();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Global Error Handling - using shared infrastructure
builder.Services.AddGlobalErrorHandling("ProperTea.Organization.Api");

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
    return cosmosClient.GetContainer("ProperTea", "Organizations");
});

// Add custom CQRS
builder.Services.AddProperCqrs();

// Add command and query handlers
builder.Services.AddCommandHandlers(typeof(CreateOrganizationCommandHandler));
builder.Services.AddQueryHandlers(typeof(CheckOrganizationExistsQueryHandler));

// Add repositories
builder.Services.AddScoped<IOrganizationRepository, CosmosOrganizationRepository>();

var app = builder.Build();

// Global Error Handling - using shared infrastructure
app.UseGlobalErrorHandling("ProperTea.Organization.Api");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapOrganizationEndpoints();

app.MapDefaultEndpoints();

app.Run();
