using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ProperTea.Identity.Api;
using System.Security.Claims;

namespace ProperTea.Identity.Api.Endpoints
{
    public static class ChangeUserIdentityPasswordEndpoint
    {
        public static void MapChangeSystemUserIdentityPasswordEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPut("/user-identity/change-password", 
                async (UserManager<UserIdentity> userManager, string currentPassword, string newPassword, HttpContext httpContext) =>
                {
                    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId == null)
                        return Results.Unauthorized();
                    var user = await userManager.FindByIdAsync(userId);
                    if (user == null)
                        return Results.NotFound();
                    var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                    return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
                }).RequireAuthorization();
        }
    }
}

