using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class CheckUserExistsActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CheckUserExistsActivity> _logger;

    public CheckUserExistsActivity(HttpClient httpClient, ILogger<CheckUserExistsActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("CheckUserExistsActivity")]
    public async Task<bool> RunAsync([ActivityTrigger] string email)
    {
        _logger.LogInformation("Checking if user exists: {Email}", email);
        
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/exists?email={Uri.EscapeDataString(email)}");
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<bool>();
            _logger.LogInformation("User exists check for {Email}: {Exists}", email, result);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check if user exists: {Email}", email);
            throw;
        }
    }
}
