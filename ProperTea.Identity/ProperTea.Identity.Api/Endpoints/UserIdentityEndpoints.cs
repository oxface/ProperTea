namespace ProperTea.Identity.Api.Endpoints;

public static class UserIdentityEndpoints
{
    public static void MapUserIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateUserIdentityEndpoint();
        endpoints.MapLoginIdentityEndpoint();
        endpoints.MapChangeSystemUserIdentityPasswordEndpoint();
        endpoints.MapDeleteIdentityEndpoint();
    }
}