using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class AddUserToOrganizationActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AddUserToOrganizationActivity> _logger;

    public AddUserToOrganizationActivity(HttpClient httpClient, ILogger<AddUserToOrganizationActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("AddUserToOrganizationActivity")]
    public async Task RunAsync([ActivityTrigger] AddUserToOrganizationRequest request)
    {
        _logger.LogInformation("Adding user {UserId} to organization {OrganizationId} with role {Role}", 
            request.UserId, request.OrganizationId, request.Role);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users/add-to-organization", new
            {
                userId = request.UserId,
                organizationId = request.OrganizationId,
                role = request.Role
            });
            
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("User added to organization successfully: {UserId} -> {OrganizationId}", 
                request.UserId, request.OrganizationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user to organization: {UserId} -> {OrganizationId}", 
                request.UserId, request.OrganizationId);
            throw;
        }
    }

    [Function("CompensateAddUserToOrganizationActivity")]
    public async Task CompensateAsync([ActivityTrigger] RemoveUserFromOrganizationRequest request)
    {
        _logger.LogInformation("Compensating user organization membership: {UserId} from {OrganizationId}", 
            request.UserId, request.OrganizationId);
        
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{request.UserId}/organizations/{request.OrganizationId}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User organization membership compensated successfully: {UserId}", request.UserId);
            }
            else
            {
                _logger.LogWarning("Failed to compensate user organization membership: {UserId}", request.UserId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compensate user organization membership: {UserId}", request.UserId);
            // Don't throw - compensations should be best effort
        }
    }
}

public record AddUserToOrganizationRequest(Guid UserId, Guid OrganizationId, string Role);
public record RemoveUserFromOrganizationRequest(Guid UserId, Guid OrganizationId);
