namespace ProperTea.SystemOwner.Api.SystemOwner.Endpoints;

public static class SystemOwnerEndpointGroup
{
    public static IEndpointRouteBuilder MapSystemOwnerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        GetSystemOwnersEndpoint.Map(endpoints);
        GetSystemOwnerByIdEndpoint.Map(endpoints);
        CreateSystemOwnerEndpoint.Map(endpoints);
        DeleteSystemOwnerEndpoint.Map(endpoints);
        ChangeSystemOwnerNameEndpoint.Map(endpoints);
        return endpoints;
    }
}