using ProperTea.Company.Infrastructure.Data;
using ProperTea.Company.MigrationService;
using ProperTea.Shared.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<CompanyDbContext>("propertea-company-db");

var host = builder.Build();
host.Run();