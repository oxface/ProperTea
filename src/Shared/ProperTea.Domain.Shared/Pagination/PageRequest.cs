namespace ProperTea.Domain.Shared.Pagination;

public record struct PageRequest(int PageNumber, int PageSize)
{
    public static PageRequest Default => new(1, 10);
}