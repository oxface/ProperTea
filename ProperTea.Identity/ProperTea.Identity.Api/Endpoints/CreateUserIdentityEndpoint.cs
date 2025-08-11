using Microsoft.AspNetCore.Identity;

namespace ProperTea.Identity.Api.Endpoints;

public static class CreateUserIdentityEndpoint
{
    public static void MapCreateUserIdentityEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // TODO: authorize this endpoint
        endpoints.MapPost("/user-identity",
            async (UserManager<UserIdentity> userManager, CreateUserIdentityRequest request) =>
            {
                var user = new UserIdentity
                {
                    Email = request.Email,
                    UserName = request.Email
                };
                var result = await userManager.CreateAsync(user, request.Password);
                return result.Succeeded ? Results.Ok(user.Id) : Results.BadRequest(result.Errors);
            });
    }
    
    public record CreateUserIdentityRequest(Guid SystemUserId, string Email, string Password);
}