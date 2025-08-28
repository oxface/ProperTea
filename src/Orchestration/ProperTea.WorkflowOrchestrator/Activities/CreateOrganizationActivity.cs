using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class CreateOrganizationActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CreateOrganizationActivity> _logger;

    public CreateOrganizationActivity(HttpClient httpClient, ILogger<CreateOrganizationActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("CreateOrganizationActivity")]
    public async Task<Guid> RunAsync([ActivityTrigger] CreateOrganizationActivityRequest request)
    {
        _logger.LogInformation("Creating organization: {Name}", request.Name);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/organizations", new
            {
                name = request.Name,
                description = request.Description,
                createdByUserId = request.CreatedByUserId
            });
            
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateOrganizationActivityResponse>();
            
            _logger.LogInformation("Organization created successfully: {OrganizationId}", result!.OrganizationId);
            return result.OrganizationId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create organization: {Name}", request.Name);
            throw;
        }
    }

    [Function("CompensateCreateOrganizationActivity")]
    public async Task CompensateAsync([ActivityTrigger] Guid organizationId)
    {
        _logger.LogInformation("Compensating organization creation: {OrganizationId}", organizationId);
        
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/organizations/{organizationId}");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Organization creation compensated successfully: {OrganizationId}", organizationId);
            }
            else
            {
                _logger.LogWarning("Failed to compensate organization creation, organization may not exist: {OrganizationId}", organizationId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compensate organization creation: {OrganizationId}", organizationId);
            // Don't throw - compensations should be best effort
        }
    }
}

public record CreateOrganizationActivityRequest(string Name, string Description, Guid CreatedByUserId);
public record CreateOrganizationActivityResponse(Guid OrganizationId);
