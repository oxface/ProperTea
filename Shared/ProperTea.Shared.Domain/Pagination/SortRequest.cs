namespace ProperTea.Shared.Domain.Pagination;

public record SortField(string PropertyName, bool IsDescending = false);

public record SortRequest
{
    public IReadOnlyList<SortField> Fields { get; }

    private SortRequest(IEnumerable<SortField> fields)
    {
        Fields = fields.ToList();
    }

    public static SortRequest By(string propertyName, bool isDescending = false)
        => new([new SortField(propertyName, isDescending)]);

    public static SortRequest By(params SortField[] fields)
        => new(fields);

    public static SortRequest By(IEnumerable<SortField> fields)
        => new(fields);
}
