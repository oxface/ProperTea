using ProperTea.Company.Domain.DomainEvents;
using ProperTea.Company.Domain.ValueObjects;

namespace ProperTea.Company.Domain;

public class Company : AggregateRootBase, ISystemOwnerScoped
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private Company()
    {
    }

    private Company(Guid id, CompanyName name, Guid systemOwnerId)
    {
        Id = id;
        Name = name;
        SystemOwnerId = systemOwnerId;
    }

    public CompanyName Name { get; private set; } = null!;

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