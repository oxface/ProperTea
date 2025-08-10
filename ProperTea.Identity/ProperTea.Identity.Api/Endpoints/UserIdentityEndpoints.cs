using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;

using ProperTea.Identity.Api;

namespace ProperTea.Identity.Api.Endpoints
{
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
}
