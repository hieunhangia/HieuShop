using Domain.Enums;

namespace Application.Common.Models;

public class PagedAndSortedResult<T>(
    IReadOnlyList<T> items,
    int totalCount,
    int pageIndex,
    int pageSize,
    string sortColumn,
    SortDirection sortDirection)
{
    public IReadOnlyList<T> Items { get; set; } = items;
    public int TotalCount { get; set; } = totalCount;
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public string SortColumn { get; set; } = sortColumn;
    public SortDirection SortDirection { get; set; } = sortDirection;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}