using ProperTea.Shared.ServiceDefaults;
using ProperTea.SystemOwner.Infrastructure.Data;
using ProperTea.SystemOwner.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<SystemOwnerDbContext>("propertea-systemowner-db");