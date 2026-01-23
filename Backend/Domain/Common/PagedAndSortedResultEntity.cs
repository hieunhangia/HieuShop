using Domain.Enums;

namespace Domain.Common;

public class PagedAndSortedResultEntity<T>(
    List<T> items,
    long totalCount,
    int pageIndex,
    int pageSize,
    SortDirection sortDirection)
{
    public List<T> Items { get; set; } = items;
    public long TotalCount { get; set; } = totalCount;
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public string SortDirection { get; set; } = sortDirection.ToString().ToLower();
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}