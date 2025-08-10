using Microsoft.AspNetCore.Identity;

namespace ProperTea.Identity.Api.Endpoints;

public static class DeleteUserIdentityEndpoint
{
    public static void MapDeleteIdentityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/user-identity/{id}", async (UserManager<UserIdentity> userManager, string id) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return Results.NotFound();
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
        }).RequireAuthorization();
    }
}