using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProperTea.Infrastructure.Shared.Extensions;
using ProperTea.ServiceDefaults;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddOpenApi();

builder.Services.AddGlobalErrorHandling("ProperTea.Gateway");

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

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

var corsAllowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBFFs", policy =>
    {
        policy.WithOrigins(corsAllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var rateLimitSection = builder.Configuration.GetSection("RateLimiting");
var globalLimit = rateLimitSection.GetValue("GlobalLimit", 100);
var windowMinutes = rateLimitSection.GetValue("WindowMinutes", 1);

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = globalLimit,
                Window = TimeSpan.FromMinutes(windowMinutes)
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddHealthChecks()
    .AddCheck("gateway-health",
        () => HealthCheckResult.Healthy("Gateway is running"), ["ready"]);

if (builder.Environment.IsDevelopment())
    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.All;
        options.RequestBodyLogLimit = 4096;
        options.ResponseBodyLogLimit = 4096;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseGlobalErrorHandling("ProperTea.Gateway");

app.UseRouting();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

app.UseCors("AllowBFFs");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
    context.Response.Headers.Append("X-Correlation-ID", correlationId);

    using var scope = app.Services.GetRequiredService<ILogger<Program>>()
        .BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId });

    await next();
});

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var route = context.GetReverseProxyFeature()?.Route;

        logger.LogInformation("Proxying request to cluster {ClusterId} for path {RequestPath} with method {HttpMethod}",
            route?.Config?.ClusterId ?? "unknown",
            context.Request.Path,
            context.Request.Method);

        await next();

        logger.LogInformation("Proxy response completed with status {StatusCode} for cluster {ClusterId}",
            context.Response.StatusCode,
            route?.Config?.ClusterId ?? "unknown");
    });
});

app.MapGet("/", () => new
{
    Service = "ProperTea API Gateway",
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.UtcNow
});

app.Run();