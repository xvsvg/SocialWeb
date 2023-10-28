namespace Contracts.Common;

public sealed class PagedResponse<T>
{
    public int RecordPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public IReadOnlyCollection<T> Bunch { get; set; } = Array.Empty<T>();
}