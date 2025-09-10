namespace ProperTea.Domain.Shared;

public interface IOrganizationScoped
{
    Guid OrganizationId { get; }
}