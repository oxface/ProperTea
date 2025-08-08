using Microsoft.EntityFrameworkCore;

using ProperTea.SystemOwner.Infrastructure.Data;

namespace ProperTea.SystemOwner.Api.Setup;

public static class DataServices
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SystemOwnerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("propertea-systemowner-db")));
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<SystemOwnerDbContext>());

        return services;
    }
}