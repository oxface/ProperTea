namespace ProperTea.Contracts.CQRS;

public interface ICommand;

public interface IQuery<TResult>;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}

public interface ICommandBus
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand;
}

public interface IQueryBus
{
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
