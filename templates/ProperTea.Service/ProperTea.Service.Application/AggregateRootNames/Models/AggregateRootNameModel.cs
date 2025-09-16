using ProperTea.Service.Domain.AggregateRootNames;

namespace ProperTea.Service.Application.AggregateRootNames.Models;

public record class AggregateRootNameModel(
    Guid Id)
{
    public static AggregateRootNameModel FromEntity(AggregateRootName user)
    {
        return new AggregateRootNameModel(
            user.Id);
    }
}