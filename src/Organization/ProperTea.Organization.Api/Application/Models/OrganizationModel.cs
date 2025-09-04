namespace ProperTea.Organization.Api.Application.Models;

public record OrganizationModel(Guid Id, string Name, string Description)
{
    public static OrganizationModel FromEntity(Domain.Organizations.Organization organization)
    {
        return new OrganizationModel(
            organization.Id,
            organization.Name,
            organization.Description);
    }
}