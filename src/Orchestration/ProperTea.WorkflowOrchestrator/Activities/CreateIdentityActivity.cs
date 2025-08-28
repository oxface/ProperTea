using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class CreateIdentityActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CreateIdentityActivity> _logger;

    public CreateIdentityActivity(HttpClient httpClient, ILogger<CreateIdentityActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("CreateIdentityActivity")]
    public async Task RunAsync([ActivityTrigger] CreateIdentityRequest request)
    {
        _logger.LogInformation("Creating identity for user: {UserId}", request.UserId);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/identities", new
            {
                userId = request.UserId,
                email = request.Email,
                password = request.Password
            });
            
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Identity created successfully for user: {UserId}", request.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create identity for user: {UserId}", request.UserId);
            throw;
        }
    }

    [Function("CompensateCreateIdentityActivity")]
    public async Task CompensateAsync([ActivityTrigger] Guid userId)
    {
        _logger.LogInformation("Compensating identity creation for user: {UserId}", userId);
        
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/identities/by-user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Identity creation compensated successfully for user: {UserId}", userId);
            }
            else
            {
                _logger.LogWarning("Failed to compensate identity creation, identity may not exist for user: {UserId}", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compensate identity creation for user: {UserId}", userId);
            // Don't throw - compensations should be best effort
        }
    }
}

public record CreateIdentityRequest(Guid UserId, string Email, string Password);
