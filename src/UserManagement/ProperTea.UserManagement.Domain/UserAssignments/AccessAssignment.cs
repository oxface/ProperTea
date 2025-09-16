using ProperTea.Shared.Domain;
using ProperTea.Shared.Domain.Events;
using ProperTea.Shared.Domain.Exceptions;

namespace ProperTea.UserManagement.Domain.UserAssignments;

public class AccessAssignment(Guid id) : AggregateRoot(id)
{
    private AccessAssignment() : this(Guid.Empty)
    {
    }

    private AccessAssignment(Guid id,
        Guid? userGroupId,
        Guid? userId,
        Guid resourceId,
        AccessAssignmentResources resourceType,
        AccessAssignmentPermissions permissionType)
        : this(id)
    {
        UserGroupId = userGroupId;
        UserId = userId;
        ResourceId = resourceId;
        ResourceType = resourceType;
        PermissionType = permissionType;

        RaiseDomainEvent(new AccessAssignmentCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            UserGroupId,
            UserId,
            ResourceId,
            ResourceType,
            PermissionType));
    }

    public Guid? UserGroupId { get; }
    public Guid? UserId { get; }
    public Guid ResourceId { get; }

    public AccessAssignmentResources ResourceType { get; } = AccessAssignmentResources.None;
    public AccessAssignmentPermissions PermissionType { get; private set; } = AccessAssignmentPermissions.None;

    public AccessAssignment Create(
        Guid? userGroupId,
        Guid? userId,
        Guid resourceId,
        AccessAssignmentResources resourceType,
        AccessAssignmentPermissions permissionType)
    {
        if (!userGroupId.HasValue && !userId.HasValue)
            throw new DomainException("AccessAssignment.UserOrGroupRequired");
        if (resourceType == AccessAssignmentResources.None)
            throw new DomainException("AccessAssignment.ResourceTypeRequired");
        if (permissionType == AccessAssignmentPermissions.None)
            throw new DomainException("AccessAssignment.PermissionTypeRequired");

        return new AccessAssignment(
            Guid.NewGuid(),
            userGroupId,
            userId,
            resourceId,
            resourceType,
            permissionType);
    }

    public void ChangePermissionType(AccessAssignmentPermissions permissionType)
    {
        PermissionType = permissionType;

        RaiseDomainEvent(new AccessAssignmentPermissionTypeChangedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id,
            PermissionType));
    }
}

public enum AccessAssignmentResources
{
    None = 0,
    Organization = 1
}

public enum AccessAssignmentPermissions
{
    None = 0,
    General = 1
}

public record AccessAssignmentCreatedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid? AccessAssignmentId,
    Guid? UserGroupId,
    Guid? UserId,
    Guid ResourceId,
    AccessAssignmentResources ResourceType,
    AccessAssignmentPermissions PermissionType
) : DomainEvent(Id, OccurredAt);

public record AccessAssignmentPermissionTypeChangedDomainEvent(
    Guid Id,
    DateTime OccurredAt,
    Guid? AccessAssignmentId,
    AccessAssignmentPermissions PermissionType
) : DomainEvent(Id, OccurredAt);