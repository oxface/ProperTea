using ProperTea.Shared.Application.Commands;

namespace ProperTea.Organization.Application.Commands;

public class ChangeOrganizationNameCommand : ICommand
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}