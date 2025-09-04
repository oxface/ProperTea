namespace ProperTea.Organization.Api.Endpoints;

public static class OrganizationEndpoints
{
    public static void MapOrganizationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/organizations")
            .WithTags("Organizations")
            .RequireAuthorization();

        // Map individual endpoints
        CreateOrganizationEndpoint.Map(app);
        ActivateOrganizationEndpoint.Map(app);
        GetOrganizationByIdEndpoint.Map(app);
        GetOrganizationByNameEndpoint.Map(app);
        CheckOrganizationExistsEndpoint.Map(app);
    }
}