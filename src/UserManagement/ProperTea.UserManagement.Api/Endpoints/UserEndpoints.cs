namespace ProperTea.UserManagement.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        CreateUserEndpoint.Map(app);
        AddUserToOrganizationEndpoint.Map(app);
        GetUserByIdEndpoint.Map(app);
        GetUserByEmailEndpoint.Map(app);
        CheckUserExistsEndpoint.Map(app);
    }
}