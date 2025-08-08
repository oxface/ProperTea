using ProperTea.Company.Domain.Company.DomainEvents;
using ProperTea.Company.Domain.Company.ValueObjects;

namespace ProperTea.Company.Domain.Company;

public class Company : AggregateRootBase, ISystemOwnerScoped
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private CompanyName _name = null!;

    private Company()
    {
    }

    private Company(Guid id, CompanyName name, Guid systemOwnerId)
    {
        Id = id;
        _name = name;
        SystemOwnerId = systemOwnerId;
    }

    public CompanyName Name
    {
        get => _name;
        private set => _name = CompanyName.Create(value);
    }

    public Guid SystemOwnerId { get; }

    public static Company Create(string name, Guid systemOwnerId)
    {
        var companyName = CompanyName.Create(name);
        var company = new Company(Guid.NewGuid(), companyName, systemOwnerId);
        company.AddDomainEvent(new CompanyCreatedDomainEvent(company.Id, company.Name));
        return company;
    }

    public void ChangeName(string newName)
    {
        var companyName = CompanyName.Create(newName);
        if (_name == companyName)
            return;

        _name = companyName;
        AddDomainEvent(new CompanyNameChangedDomainEvent(Id, Name));
    }

    public bool AllowDelete()
    {
        return true;
    }
}