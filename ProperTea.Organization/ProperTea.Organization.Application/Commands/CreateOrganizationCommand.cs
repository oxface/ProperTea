using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class CreateOrganizationCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}