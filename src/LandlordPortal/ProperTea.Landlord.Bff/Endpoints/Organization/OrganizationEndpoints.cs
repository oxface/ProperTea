namespace ProperTea.Landlord.Bff.Endpoints.Organization;

public static class OrganizationEndpoints
{
    public static void MapOrganizationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/organizations")
            .WithTags("Organizations")
            .RequireAuthorization();
        
        CreateOrganizationEndpoint.Map(app);
    }
}
