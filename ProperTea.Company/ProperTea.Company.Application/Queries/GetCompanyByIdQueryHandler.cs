using ProperTea.Company.Application.Models;
using ProperTea.Company.Domain;
using ProperTea.Shared.Application.Queries;

namespace ProperTea.Company.Application.Queries;

public class GetCompanyByIdQueryHandler(ICompanyRepository repository)
    : IQueryHandler<GetCompanyByIdQuery, CompanyModel>
{
    public async Task<CompanyModel> HandleAsync(GetCompanyByIdQuery query, CancellationToken ct)
    {
        var company = await repository.GetByIdAsync(query.Id, ct);
        if (company == null)
            throw new Exception("Company not found");
        return new CompanyModel
        {
            Id = company.Id,
            Name = company.Name
        };
    }
}