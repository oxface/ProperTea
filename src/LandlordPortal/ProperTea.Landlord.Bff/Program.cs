using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Timeouts;
using Polly;
using Polly.Extensions.Http;
using ProperTea.Infrastructure.Shared.Extensions;
using ProperTea.Landlord.Bff.Endpoints.Organization;
using ProperTea.Landlord.Bff.Endpoints.User;
using ProperTea.ServiceDefaults;
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

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

builder.Services.AddHttpClient("gateway", client =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "ProperTea-LandlordBff/1.0");
        client.BaseAddress = new Uri("https://gateway");
    })
    .AddServiceDiscovery()
    .AddPolicyHandler(retryPolicy);

builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy =
        new RequestTimeoutPolicy { Timeout = TimeSpan.FromMilliseconds(300) };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("LandlordPortalPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                             ?? new[] { "http://localhost:3000", "https://localhost:3001" };

        policy.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.Landlord.Bff");

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("LandlordPortalPolicy");

app.UseGlobalErrorHandling("ProperTea.Landlord.Bff");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapOrganizationEndpoints();
app.MapUserEndpoints();

app.MapDefaultEndpoints();

app.Run();