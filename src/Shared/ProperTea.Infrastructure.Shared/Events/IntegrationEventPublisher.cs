using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using ProperTea.Contracts.Events;
using System.Text.Json;

namespace ProperTea.Infrastructure.Shared.Events;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IntegrationEvent;
}

public class ServiceBusIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly ServiceBusSender _sender;
    private readonly ILogger<ServiceBusIntegrationEventPublisher> _logger;

    public ServiceBusIntegrationEventPublisher(ServiceBusSender sender, ILogger<ServiceBusIntegrationEventPublisher> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IntegrationEvent
    {
        try
        {
            var messageBody = JsonSerializer.Serialize(@event);
            var message = new ServiceBusMessage(messageBody)
            {
                Subject = typeof(TEvent).Name,
                MessageId = @event.Id.ToString(),
                CorrelationId = @event.Id.ToString()
            };

            await _sender.SendMessageAsync(message, cancellationToken);
            _logger.LogInformation("Published integration event {EventType} with ID {EventId}", 
                typeof(TEvent).Name, @event.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish integration event {EventType} with ID {EventId}", 
                typeof(TEvent).Name, @event.Id);
            throw;
        }
    }
}
