using Microsoft.Extensions.DependencyInjection;

namespace ProperTea.Cqrs;

public class CommandBus : ICommandBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CommandBus(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }
}

public class QueryBus : IQueryBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public QueryBus(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }
}