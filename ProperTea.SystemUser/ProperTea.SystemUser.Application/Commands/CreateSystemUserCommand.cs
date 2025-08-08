using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemUser.Application.Commands;

public class CreateSystemUserCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}