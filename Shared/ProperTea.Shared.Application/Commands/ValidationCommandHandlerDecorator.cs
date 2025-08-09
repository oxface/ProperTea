using FluentValidation;

namespace ProperTea.Shared.Application.Commands;

public class ValidationCommandHandlerDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> decorated,
    IValidator<TCommand>? validator = null)
    : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand
{
    public async Task<TResult> HandleAsync(TCommand command)
    {
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }

        return await decorated.HandleAsync(command);
    }
}

public class ValidationCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> decorated,
    IValidator<TCommand>? validator = null)
    :
        ValidationCommandHandlerDecorator<TCommand, object?>(decorated, validator), ICommandHandler<TCommand>
    where TCommand : ICommand;