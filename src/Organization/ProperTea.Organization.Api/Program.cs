using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Handlers;
using ProperTea.Organization.Api.Domain.Organizations;
using ProperTea.Organization.Api.Endpoints;
using ProperTea.Organization.Api.Infrastructure.Persistence;
using ProperTea.ServiceDefaults;
using ProperTea.Shared.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

builder.Services.AddGlobalErrorHandling("ProperTea.Organization.Api");

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb")
                           ?? throw new InvalidOperationException("CosmosDb connection string is not configured.");
    return new CosmosClient(connectionString);
});

builder.Services.AddProperCqrs();
builder.Services.AddProperCqrsCommandHandlers(typeof(ActivateOrganizationCommandHandler).Assembly);
builder.Services.AddProperCqrsQueryHandlers(typeof(CheckOrganizationExistsQueryHandler).Assembly);

builder.Services.AddScoped<IOrganizationRepository, CosmosOrganizationRepository>(f =>
    new CosmosOrganizationRepository(f.GetRequiredService<CosmosClient>(),
        "propertea-user-management-db",
        "users"));

var app = builder.Build();

app.UseGlobalErrorHandling("ProperTea.Organization.Api");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapOrganizationEndpoints();

app.MapDefaultEndpoints();

app.Run();