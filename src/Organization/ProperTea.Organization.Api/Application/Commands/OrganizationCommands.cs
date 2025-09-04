using ProperTea.Cqrs;

namespace ProperTea.Organization.Api.Application.Commands;

public record CreateOrganizationCommand(
    string Name,
    string Description,
    Guid CreatedByUserId
) : ICommand;

public record ActivateOrganizationCommand(
    Guid OrganizationId
) : ICommand;