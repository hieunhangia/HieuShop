namespace Domain.Common;

public class PagedResultEntity<T>(List<T> items, long totalCount, int pageIndex, int pageSize)
{
    public List<T> Items { get; set; } = items;
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public long TotalCount { get; set; } = totalCount;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}