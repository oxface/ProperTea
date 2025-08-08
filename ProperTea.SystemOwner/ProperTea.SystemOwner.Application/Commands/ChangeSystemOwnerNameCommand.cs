using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemOwner.Application.Commands;

public class ChangeSystemOwnerNameCommand : ICommand
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}