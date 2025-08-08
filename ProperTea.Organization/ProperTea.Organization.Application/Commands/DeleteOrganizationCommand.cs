using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class DeleteOrganizationCommand : ICommand
{
    public Guid Id { get; set; }
}