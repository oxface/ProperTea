using ProperTea.Shared.Application.Commands;

namespace ProperTea.SystemUser.Application.Commands;

public class ChangeSystemUserNameCommand : ICommand
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}