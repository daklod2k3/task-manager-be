using System.Linq.Expressions;

namespace server.Helpers;

public class CompositeFilter<T>
{
    public static Expression<Func<T, bool>> ApplyFilter(ClientFilter filter)
    {
        if (filter == null || filter.Filters == null || !filter.Filters.Any())
            return null;

        Expression<Func<T, bool>> compositeFilterExpression = null;
        if (filter.Filters.Count == 1)
            compositeFilterExpression = GetSingleFilterExpression(filter.Filters.First());
        else if (filter.Logic?.ToLower() == "and")
            compositeFilterExpression = GetAndFilterExpression(filter.Filters);
        else if (filter.Logic?.ToLower() == "or") compositeFilterExpression = GetOrFilterExpression(filter.Filters);

        return compositeFilterExpression;
    }

    private static Expression<Func<T, bool>> GetSingleFilterExpression(Filter filter)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var filterExpression = BuildFilterExpression(filter, parameter);
        return Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
    }

    private static Expression<Func<T, bool>> GetAndFilterExpression(List<Filter> filters)
    {
        if (filters == null || !filters.Any())
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression andExpression = null;

        foreach (var filter in filters)
        {
            var filterExpression = BuildFilterExpression(filter, parameter);
            if (filterExpression != null)
            {
                if (andExpression == null)
                    andExpression = filterExpression;
                else
                    andExpression = Expression.AndAlso(andExpression, filterExpression);
            }
        }

        if (andExpression == null)
            // Return default expression that always evaluates to false
            andExpression = Expression.Constant(false);

        return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
    }

    private static Expression<Func<T, bool>> GetOrFilterExpression(List<Filter> filters)
    {
        if (filters == null || !filters.Any())
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression orExpression = null;

        foreach (var filter in filters)
        {
            var filterExpression = BuildFilterExpression(filter, parameter);
            if (filterExpression != null)
            {
                if (orExpression == null)
                    orExpression = filterExpression;
                else
                    orExpression = Expression.OrElse(orExpression, filterExpression);
            }
        }

        if (orExpression == null)
            // Return default expression that always evaluates to false
            orExpression = Expression.Constant(false);

        return Expression.Lambda<Func<T, bool>>(orExpression, parameter);
    }

    private static Expression BuildFilterExpression(Filter filter, ParameterExpression parameter)
    {
        if (filter.Filters != null && filter.Filters.Any())
        {
            if (filter.Logic?.ToLower() == "and")
            {
                var andFilters = filter.Filters.Select(f => BuildFilterExpression(f, parameter));
                return andFilters.Aggregate(Expression.AndAlso);
            }

            if (filter.Logic?.ToLower() == "or")
            {
                var orFilters = filter.Filters.Select(f => BuildFilterExpression(f, parameter));
                return orFilters.Aggregate(Expression.OrElse);
            }
        }

        if (filter.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString()))
            return null;
        var property = Expression.Property(parameter, filter.Field);
        var constant = Expression.Constant(filter.Value);

        if (property.Type.IsEnum || Nullable.GetUnderlyingType(property.Type)?.IsEnum == true)
        {
            var enumType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            var enumValue = Enum.Parse(enumType, filter.Value.ToString(), true);
            constant = Expression.Constant(enumValue, property.Type);
        }

        switch (filter.Operator.ToLower())
        {
            case "eq":
                return Expression.Equal(property, constant);
            case "neq":
                return Expression.NotEqual(property, constant);
            case "lt":
                return Expression.LessThan(property, constant);
            case "lte":
                return Expression.LessThanOrEqual(property, constant);
            case "gt":
                return Expression.GreaterThan(property, constant);
            case "gte":
                return Expression.GreaterThanOrEqual(property, constant);
            case "ctn":

                // Check if the property is a string
                if (property.Type == typeof(string))
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

                    // Apply ToLower() only if the property is a string
                    var propertyLower = Expression.Call(property, toLowerMethod);
                    var constantLower = Expression.Call(constant, toLowerMethod);
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                    // Example: Use the lowercased versions in an equality expression
                    return Expression.Call(propertyLower, containsMethod, constantLower);
                }

                return null;

            case "stw":

// Check if the property is a string
                if (property.Type == typeof(string))
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

                    // Apply ToLower() only if the property is a string
                    var propertyLower = Expression.Call(property, toLowerMethod);
                    var constantLower = Expression.Call(constant, toLowerMethod);

                    // Example: Use the lowercased versions in an equality expression
                    var startsWithMethod =
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });

                    // Convert the constant value to lowercase for case-insensitive comparison
                    // return Expression.Lambda()
                    return Expression.Call(propertyLower, startsWithMethod, constantLower);
                }

                return null;


            // Add more operators as needed...
            default:
                throw new ArgumentException($"Unsupported operator: {filter.Operator}");
        }
    }
}