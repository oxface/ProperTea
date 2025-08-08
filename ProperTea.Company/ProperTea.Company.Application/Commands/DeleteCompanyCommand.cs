using ProperTea.Shared.Application.Commands;

namespace ProperTea.Company.Application.Commands;

public class DeleteCompanyCommand : ICommand
{
    public Guid Id { get; set; }
}