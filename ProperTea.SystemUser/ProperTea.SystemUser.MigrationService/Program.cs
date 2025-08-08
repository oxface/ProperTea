using ProperTea.Shared.ServiceDefaults;
using ProperTea.SystemUser.Infrastructure.Data;
using ProperTea.SystemUser.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<SystemUserDbContext>("propertea-systemuser-db");

var host = builder.Build();
host.Run();