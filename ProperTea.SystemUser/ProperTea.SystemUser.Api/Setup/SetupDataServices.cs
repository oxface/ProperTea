using Microsoft.EntityFrameworkCore;

using ProperTea.SystemUser.Infrastructure.Data;

namespace ProperTea.SystemUser.Api.Setup;

public static class DataServices
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SystemUserDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("propertea-systemuser-db")));
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<SystemUserDbContext>());

        return services;
    }
}