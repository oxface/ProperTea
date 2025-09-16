using ProperTea.ProperCqrs;
using ProperTea.Service.Domain.AggregateRootNames;
using ProperTea.Shared.Application;

namespace ProperTea.Service.Application.AggregateRootNames.Commands;

public record CreateAggregateRootNameCommand : ICommand;

public class CreateUserCommandHandler(
    IAggregateRootNameDomainService domainService,
    IUnitOfWork unitOfWork)
    : CommandHandler<CreateAggregateRootNameCommand>
{
    public override async Task HandleAsync(CreateAggregateRootNameCommand command, CancellationToken ct = default)
    {
        // TODO: Implement command handler
        await unitOfWork.SaveAsync(ct);
    }
}