namespace ProperTea.WorkflowOrchestrator.Services;

public interface IGatewayClient
{
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
}

public class GatewayClient : IGatewayClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GatewayClient> _logger;

    public GatewayClient(HttpClient httpClient, ILogger<GatewayClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Making GET request to Gateway: {Endpoint}", endpoint);
            
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            }
            
            _logger.LogWarning("Gateway GET request failed. Endpoint: {Endpoint}, Status: {StatusCode}", 
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making GET request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint, 
        TRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Making POST request to Gateway: {Endpoint}", endpoint);
            
            var response = await _httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
            }
            
            _logger.LogWarning("Gateway POST request failed. Endpoint: {Endpoint}, Status: {StatusCode}", 
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error making POST request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }
}
