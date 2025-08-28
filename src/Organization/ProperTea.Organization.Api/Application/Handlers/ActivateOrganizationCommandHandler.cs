using ProperTea.Contracts.CQRS;
using ProperTea.Organization.Api.Application.Commands;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class ActivateOrganizationCommandHandler : ICommandHandler<ActivateOrganizationCommand>
{
    private readonly IOrganizationRepository _organizationRepository;

    public ActivateOrganizationCommandHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task HandleAsync(ActivateOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var organization = await _organizationRepository.GetByIdAsync(command.OrganizationId, cancellationToken);
        if (organization == null)
        {
            throw new InvalidOperationException($"Organization with ID '{command.OrganizationId}' not found");
        }

        organization.Activate();
        await _organizationRepository.SaveAsync(organization, cancellationToken);
    }
}
