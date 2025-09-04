using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace ProperTea.Shared.Infrastructure.Extensions;

public static class HttpClientExtensions
{
    public static async Task<T?> GetAsync<T>(this HttpClient httpClient,
        string endpoint,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Making GET request to Gateway: {Endpoint}", endpoint);

            var response = await httpClient.GetAsync(endpoint, cancellationToken);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<T>(cancellationToken);

            logger.LogWarning("Gateway GET request failed. Endpoint: {Endpoint}, Status: {StatusCode}",
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making GET request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }

    public static async Task<TResponse?> PostAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string endpoint,
        TRequest request,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Making POST request to Gateway: {Endpoint}", endpoint);

            var response = await httpClient.PostAsJsonAsync(endpoint, request, cancellationToken);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

            logger.LogWarning("Gateway POST request failed. Endpoint: {Endpoint}, Status: {StatusCode}",
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making POST request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }

    public static async Task<TResponse?> PutAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string endpoint,
        TRequest request,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Making PUT request to Gateway: {Endpoint}", endpoint);

            var response = await httpClient.PutAsJsonAsync(endpoint, request, cancellationToken);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

            logger.LogWarning("Gateway PUT request failed. Endpoint: {Endpoint}, Status: {StatusCode}",
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making POST request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }

    public static async Task<TResponse?> DeleteAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string endpoint,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogDebug("Making DELETE request to Gateway: {Endpoint}", endpoint);

            var response = await httpClient.DeleteAsync(endpoint, cancellationToken);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken);

            logger.LogWarning("Gateway DELETE request failed. Endpoint: {Endpoint}, Status: {StatusCode}",
                endpoint, response.StatusCode);
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error making POST request to Gateway: {Endpoint}", endpoint);
            return default;
        }
    }
}