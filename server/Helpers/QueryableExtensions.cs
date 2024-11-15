namespace server.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, Pagination? pagination)
        {
            var defaultPagination = pagination ?? new Pagination(); 
            var skipNumber = (defaultPagination.PageNumber - 1) * defaultPagination.PageSize;
            return source.Skip(skipNumber).Take(defaultPagination.PageSize);
        }
    }
}
