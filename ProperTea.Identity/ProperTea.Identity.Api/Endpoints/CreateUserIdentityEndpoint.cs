using Microsoft.AspNetCore.Identity;

namespace ProperTea.Identity.Api.Endpoints;

public static class CreateUserIdentityEndpoint
{
    public static void MapCreateUserIdentityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // TODO: authorize this endpoint
        endpoints.MapPost("/user-identity", async (UserManager<UserIdentity> userManager, UserIdentity user, string password) =>
        {
            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded ? Results.Created($"/user-identity/{user.Id}", user) : Results.BadRequest(result.Errors);
        });
    }
}