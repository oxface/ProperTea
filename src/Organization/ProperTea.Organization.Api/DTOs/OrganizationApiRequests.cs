namespace ProperTea.Organization.Api.DTOs;

public record CreateOrganizationApiRequest(string Name, string Description, Guid CreatedByUserId);