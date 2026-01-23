using Domain.Enums;

namespace Application.Common.Models;

public class PagedAndSortedResult<T>(
    List<T> items,
    int totalCount,
    int pageIndex,
    int pageSize,
    SortDirection sortDirection)
{
    public List<T> Items { get; set; } = items;
    public int TotalCount { get; set; } = totalCount;
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public string SortDirection { get; set; } = sortDirection.ToString().ToLower();
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}