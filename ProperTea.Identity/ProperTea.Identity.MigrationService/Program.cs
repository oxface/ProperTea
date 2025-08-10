using ProperTea.Identity.Api;
using ProperTea.Identity.MigrationService;
using ProperTea.Shared.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<UserIdentityDbContext>("propertea-identity-db");

var host = builder.Build();
host.Run();