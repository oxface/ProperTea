using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemOwner.Application.Commands;

public class CreateSystemOwnerCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}