namespace ProperTea.UserManagement.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("UserIds");

        CreateUserEndpoint.Map(app);
        GetUserByIdEndpoint.Map(app);
        CheckUserExistsEndpoint.Map(app);
        AddUserIdentityEndpoint.Map(app);
    }
}