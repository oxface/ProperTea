using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProperTea.Identity.Api;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProperTea.Identity.Api.Endpoints
{
    public static class UserLoginEndpoint
    {
        public static void MapLoginIdentityEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/login", async (
                UserManager<UserIdentity> userManager,
                SignInManager<UserIdentity> signInManager,
                IConfiguration config,
                string username,
                string password) =>
            {
                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                    return Results.Unauthorized();
                var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
                if (!result.Succeeded)
                    return Results.Unauthorized();
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(new { token = jwt });
            });
        }
    }
}

