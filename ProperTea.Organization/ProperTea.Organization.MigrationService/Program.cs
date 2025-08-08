using ProperTea.Organization.Infrastructure.Data;
using ProperTea.Organization.MigrationService;
using ProperTea.Shared.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<OrganizationDbContext>("propertea-organization-db");

var host = builder.Build();
host.Run();