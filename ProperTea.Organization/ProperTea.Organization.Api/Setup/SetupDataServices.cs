using Microsoft.EntityFrameworkCore;

using ProperTea.Organization.Infrastructure.Data;

namespace ProperTea.Organization.Api.Setup;

public static class DataServices
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<OrganizationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("propertea-organization-db")));
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<OrganizationDbContext>());

        return services;
    }
}