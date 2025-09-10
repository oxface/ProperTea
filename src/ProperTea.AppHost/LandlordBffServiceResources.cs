using Projects;

namespace ProperTea.AppHost;

public static class LandlordBffServiceResources
{
    public static IResourceBuilder<ProjectResource> RegisterLandlordBffServiceResources(
        this IDistributedApplicationBuilder builder,
        string environment)
    {
        var landlordBff = builder.AddProject<ProperTea_Landlord_Bff>("landlord-bff")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", environment);

        return landlordBff;
    }
}