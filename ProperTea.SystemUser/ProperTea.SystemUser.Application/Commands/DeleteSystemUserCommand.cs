using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemUser.Application.Commands;

public class DeleteSystemUserCommand : ICommand
{
    public Guid Id { get; set; }
}