using ProperTea.Company.Domain.DomainEvents;
using ProperTea.Company.Domain.ValueObjects;

namespace ProperTea.Company.Domain;

public class Company : AggregateRootBase, IOrganizationScoped
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private Company()
    {
    }

    private Company(Guid id, CompanyName name, Guid organizationId)
    {
        Id = id;
        Name = name;
        OrganizationId = organizationId;
    }

    public CompanyName Name { get; private set; } = null!;

    public Guid OrganizationId { get; }

    public static Company Create(string name, Guid organizationId)
    {
        var companyName = CompanyName.Create(name);
        var company = new Company(Guid.NewGuid(), companyName, organizationId);
        company.AddDomainEvent(new CompanyCreatedDomainEvent(company.Id, company.Name));
        return company;
    }

    public void ChangeName(string newName)
    {
        var companyName = CompanyName.Create(newName);
        if (Name == companyName)
            return;

        Name = companyName;
        AddDomainEvent(new CompanyNameChangedDomainEvent(Id, Name));
    }

    public bool AllowDelete()
    {
        return true;
    }
}