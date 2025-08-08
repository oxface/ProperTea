using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemOwner.Application.SystemOwner.Commands;

public class CreateSystemOwnerCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}