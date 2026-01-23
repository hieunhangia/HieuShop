using Domain.Enums;

namespace Infrastructure.Data.Extensions;

using System.Linq.Dynamic.Core;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, SortDirection sortDirection)
    {
        var sortDirectionString = sortDirection == SortDirection.Asc ? "ASC" : "DESC";
        var orderString = $"{sortColumn} {sortDirectionString}";
        try
        {
            return query.OrderBy(orderString);
        }
        catch
        {
            return query;
        }
    }
}