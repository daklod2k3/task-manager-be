using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace server.Helpers;

public static class QueryableExtensions
{
    private static int DefaultPageSize => 50;
    private static int DefaultPageNumber => 1;

    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int? page, int? pageSize)
    {
        var defaultPagination = new Pagination(page, pageSize);
        var skipNumber = (defaultPagination.Page - 1) * defaultPagination.PageSize;
        return source.Skip(skipNumber).Take(defaultPagination.PageSize);
    }

    public static IQueryable<T> GetInclude<T>(this IQueryable<T> source, string? includeProperties)
    {
        if (!string.IsNullOrEmpty(includeProperties))
            foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                return source.Include(includeProp);
        return source;
    }

    public static IQueryable<T> GetOrderBy<T>(this IQueryable<T> source, string? orderBy)
    {
        if (!string.IsNullOrEmpty(orderBy))
        {
            var parts = orderBy.Split('_');
            if (parts.Length == 2 && parts[1] == "desc")
                return source.OrderByDescending(x => EF.Property<object>(x, parts[0]));

            return source.OrderBy(x => EF.Property<object>(x, parts[0]));
        }

        return source;
    }

    public class Pagination
    {
        public Pagination(int? page, int? pageSize)
        {
            Page = page ?? DefaultPageNumber;
            PageSize = pageSize ?? DefaultPageSize;
        }


        public int Page { get; }
        public int PageSize { get; }
    }
}