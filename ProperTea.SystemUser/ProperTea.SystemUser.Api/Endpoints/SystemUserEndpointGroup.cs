namespace ProperTea.SystemUser.Api.Endpoints;

public static class SystemUserEndpointGroup
{
    public static IEndpointRouteBuilder MapSystemUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        GetSystemUsersEndpoint.Map(endpoints);
        GetSystemUserByIdEndpoint.Map(endpoints);
        CreateSystemUserEndpoint.Map(endpoints);
        DeleteSystemUserEndpoint.Map(endpoints);
        ChangeSystemUserNameEndpoint.Map(endpoints);
        return endpoints;
    }
}