using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class CreateUserActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CreateUserActivity> _logger;

    public CreateUserActivity(HttpClient httpClient, ILogger<CreateUserActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("CreateUserActivity")]
    public async Task<Guid> RunAsync([ActivityTrigger] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user: {Email}", request.Email);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users", new
            {
                email = request.Email,
                fullName = request.FullName
            });
            
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            
            _logger.LogInformation("User created successfully: {UserId}", result!.UserId);
            return result.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user: {Email}", request.Email);
            throw;
        }
    }

    [Function("CompensateCreateUserActivity")]
    public async Task CompensateAsync([ActivityTrigger] Guid userId)
    {
        _logger.LogInformation("Compensating user creation: {UserId}", userId);
        
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User creation compensated successfully: {UserId}", userId);
            }
            else
            {
                _logger.LogWarning("Failed to compensate user creation, user may not exist: {UserId}", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compensate user creation: {UserId}", userId);
            // Don't throw - compensations should be best effort
        }
    }
}

public record CreateUserRequest(string Email, string FullName);
public record CreateUserResponse(Guid UserId);
