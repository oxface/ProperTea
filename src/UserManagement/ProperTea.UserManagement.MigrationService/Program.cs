using ProperTea.ServiceDefaults;
using ProperTea.UserManagement.Infrastructure.Persistence;
using ProperTea.UserManagement.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<DataMigrationWorker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(DataMigrationWorker.ActivitySourceName));
builder.AddSqlServerDbContext<UserManagementDbContext>("propertea-user-management-db");

var host = builder.Build();
host.Run();