using ProperTea.Organization.Domain.DomainEvents;
using ProperTea.Organization.Domain.ValueObjects;

namespace ProperTea.Organization.Domain;

public class Organization : AggregateRootBase
{
    public const int MaxNameLength = 200;
    public const int MinNameLength = 1;

    private Organization()
    {
    }

    private Organization(Guid id, OrganizationName name)
    {
        Id = id;
        Name = name;
    }

    public OrganizationName Name { get; private set; } = null!;

    public static Organization Create(string name)
    {
        var organizationName = OrganizationName.Create(name);
        var organization = new Organization(Guid.NewGuid(), organizationName);
        organization.AddDomainEvent(new OrganizationCreatedDomainEvent(organization.Id, organization.Name));
        return organization;
    }

    public void ChangeName(string newName)
    {
        var organizationName = OrganizationName.Create(newName);
        if (Name == organizationName)
            return;

        Name = organizationName;
        AddDomainEvent(new OrganizationNameChangedDomainEvent(Id, Name));
    }

    public bool AllowDelete()
    {
        return true;
    }
}