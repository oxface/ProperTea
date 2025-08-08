using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Application.Commands;

public class ChangeCompanyNameCommand : ICommand
{
    public Guid Id { get; set; }
    public string NewName { get; set; } = string.Empty;
}