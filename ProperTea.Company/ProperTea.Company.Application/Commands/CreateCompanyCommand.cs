using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Application.Commands;

public class CreateCompanyCommand : ICommand
{
    public string Name { get; set; } = string.Empty;
}