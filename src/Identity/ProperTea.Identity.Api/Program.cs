using Microsoft.Azure.Cosmos;
using ProperTea.Cqrs;
using ProperTea.Identity.Api.Application.Handlers;
using ProperTea.Identity.Api.Domain.Identities;
using ProperTea.Identity.Api.Endpoints;
using ProperTea.Identity.Api.Infrastructure.Persistence;
using ProperTea.ServiceDefaults;
using ProperTea.Shared.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.Identity.Api");

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb") ??
                           throw new InvalidOperationException("CosmosDb connection string is not configured.");
    return new CosmosClient(connectionString);
});

builder.Services.AddScoped<Container>(serviceProvider =>
{
    var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
    return cosmosClient.GetContainer("ProperTea", "Identities");
});

builder.Services.AddProperCqrs();
builder.Services.AddProperCqrsCommandHandlers(typeof(CreateIdentityCommandHandler).Assembly);
builder.Services.AddProperCqrsQueryHandlers(typeof(CreateIdentityCommandHandler).Assembly);

builder.Services.AddScoped<IUserIdentityRepository, CosmosUserIdentityRepository>();

var app = builder.Build();

app.UseGlobalErrorHandling("ProperTea.Identity.Api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapIdentityEndpoints();

app.MapDefaultEndpoints();

app.Run();