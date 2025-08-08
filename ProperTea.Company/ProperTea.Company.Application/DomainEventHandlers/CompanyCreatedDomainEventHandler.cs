using ProperTea.Company.Domain;
using ProperTea.Company.Domain.DomainEvents;
using ProperTea.Shared.Domain.DomainEvents;

namespace ProperTea.Company.Application.DomainEventHandlers;

public class CompanyCreatedDomainEventHandler(ICompanyDomainService companyDomainService)
    : IDomainEventHandler<CompanyCreatedDomainEvent>
{
    public async Task HandleAsync(CompanyCreatedDomainEvent domainEvent, CancellationToken ct = default)
    {
        // TODO: integration event.
        await companyDomainService.ChangeCompanyNameAsync(domainEvent.Id, domainEvent.Name + "1", ct);
    }
}