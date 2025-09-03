namespace ProperTea.Landlord.Bff.Endpoints.User;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");
        
        CreateUserEndpoint.Map(app);
    }
}
