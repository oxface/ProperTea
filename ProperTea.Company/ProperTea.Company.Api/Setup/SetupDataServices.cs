using Microsoft.EntityFrameworkCore;

using ProperTea.Company.Infrastructure.Data;

namespace ProperTea.Company.Api.Setup;

public static class DataServices
{
    public static IServiceCollection AddDataServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CompanyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("propertea-company-db")));
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CompanyDbContext>());

        return services;
    }
}