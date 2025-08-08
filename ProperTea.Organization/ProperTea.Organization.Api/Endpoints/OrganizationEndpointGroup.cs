namespace ProperTea.Organization.Api.Endpoints;

public static class OrganizationEndpointGroup
{
    public static IEndpointRouteBuilder MapOrganizationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        GetOrganizationEndpoint.Map(endpoints);
        GetOrganizationByIdEndpoint.Map(endpoints);
        CreateOrganizationEndpoint.Map(endpoints);
        DeleteOrganizationEndpoint.Map(endpoints);
        ChangeOrganizationNameEndpoint.Map(endpoints);
        return endpoints;
    }
}