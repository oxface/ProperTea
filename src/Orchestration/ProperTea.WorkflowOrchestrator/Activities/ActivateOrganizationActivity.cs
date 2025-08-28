using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ProperTea.WorkflowOrchestrator.Activities;

public class ActivateOrganizationActivity
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ActivateOrganizationActivity> _logger;

    public ActivateOrganizationActivity(HttpClient httpClient, ILogger<ActivateOrganizationActivity> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Function("ActivateOrganizationActivity")]
    public async Task RunAsync([ActivityTrigger] Guid organizationId)
    {
        _logger.LogInformation("Activating organization: {OrganizationId}", organizationId);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/organizations/{organizationId}/activate", new { });
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation("Organization activated successfully: {OrganizationId}", organizationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to activate organization: {OrganizationId}", organizationId);
            throw;
        }
    }

    [Function("CompensateActivateOrganizationActivity")]
    public async Task CompensateAsync([ActivityTrigger] Guid organizationId)
    {
        _logger.LogInformation("Compensating organization activation: {OrganizationId}", organizationId);
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/organizations/{organizationId}/deactivate", new { });
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Organization activation compensated successfully: {OrganizationId}", organizationId);
            }
            else
            {
                _logger.LogWarning("Failed to compensate organization activation: {OrganizationId}", organizationId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to compensate organization activation: {OrganizationId}", organizationId);
            // Don't throw - compensations should be best effort
        }
    }
}
