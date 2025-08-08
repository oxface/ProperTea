namespace ProperTea.Shared.Domain;

public interface ISystemOwnerScoped
{
    Guid SystemOwnerId { get; }
}