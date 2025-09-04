using ProperTea.Cqrs;
using ProperTea.Organization.Api.Application.Commands;
using ProperTea.Organization.Api.Domain.Organizations;

namespace ProperTea.Organization.Api.Application.Handlers;

public class CreateOrganizationCommandHandler : ICommandHandler<CreateOrganizationCommand>
{
    private readonly IOrganizationRepository _organizationRepository;

    public CreateOrganizationCommandHandler(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task HandleAsync(CreateOrganizationCommand command, CancellationToken cancellationToken = default)
    {
        var existingOrganization = await _organizationRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingOrganization != null)
            throw new InvalidOperationException($"Organization with name '{command.Name}' already exists");

        var organization = Domain.Organizations.Organization.Create(
            command.Name,
            command.Description,
            command.CreatedByUserId);

        await _organizationRepository.SaveAsync(organization, cancellationToken);
    }
}