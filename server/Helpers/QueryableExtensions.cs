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