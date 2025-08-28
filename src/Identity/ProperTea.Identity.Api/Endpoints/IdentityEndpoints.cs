namespace ProperTea.Identity.Api.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identities")
            .WithTags("Identities");

        // Map individual endpoints
        CreateIdentityEndpoint.Map(app);
        AuthenticateEndpoint.Map(app);
    }
}
