using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class GetUserByEmailActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetUserByEmailActivity> _logger;

    public GetUserByEmailActivity(HttpClient httpClient, ILogger<GetUserByEmailActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("GetUserByEmailActivity")]
    public async Task<Guid> RunAsync([ActivityTrigger] string email)
    {
        _logger.LogInformation("Getting user by email: {Email}", email);
        
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/by-email?email={Uri.EscapeDataString(email)}");
            response.EnsureSuccessStatusCode();
            
            var user = await response.Content.ReadFromJsonAsync<GetUserResponse>();
            _logger.LogInformation("User found: {UserId} for email: {Email}", user!.Id, email);
            
            return user.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by email: {Email}", email);
            throw;
        }
    }
}

public record GetUserResponse(Guid Id, string Email, string FullName);
