using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemOwner.Application.Commands;

public class DeleteSystemOwnerCommand : ICommand
{
    public Guid Id { get; set; }
}