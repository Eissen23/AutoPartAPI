using System.Linq.Expressions;
using Base.Application.Common.Models;

namespace Base.Application.Common.Extension;

public static class QueryableFilterExtensions
{
    public static IQueryable<T> ApplyBaseFilter<T>(this IQueryable<T> query, BaseFilter? baseFilter)
        where T : class
    {
        if (baseFilter is null)
            return query;

        return query
            .ApplySearch(baseFilter.AdvanceSearches)
            .ApplyFilter(baseFilter.AdvanceFilter)
            .ApplySort(baseFilter.AdvanceSort);
    }

    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, Search? search)
        where T : class
    {
        if (search is null)
            return query;

        if (string.IsNullOrWhiteSpace(search.Keyword) || search.Fields is null || search.Fields.Count == 0)
            return query;

        var keyword = search.Keyword.Trim();
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var field in search.Fields)
        {
            var fieldExpression = FilterExpressionBuilder.BuildSearchFieldExpression<T>(field, keyword, parameter);
            if (fieldExpression is null)
                continue;

            var lambda = Expression.Lambda<Func<T, bool>>(fieldExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Filter? filter)
        where T : class
    {
        if (filter is null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var filterExpression = FilterExpressionBuilder.BuildFilterExpression<T>(filter, parameter);

        if (filterExpression is not null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, Sort? sort)
        where T : class
    {
        if (sort?.SortBy is null || sort.SortBy.Count == 0)
            return query;

        IOrderedQueryable<T>? ordered = null;

        for (int i = 0; i < sort.SortBy.Count; i++)
        {
            var field = sort.SortBy[i];

            if (string.IsNullOrWhiteSpace(field))
                continue;

            var direction = sort.SortDirection is not null && i < sort.SortDirection.Count
                ? sort.SortDirection[i]
                : SortDirection.ASC;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, field);
                var convertedProperty = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<T, object?>>(convertedProperty, parameter);

                if (ordered is null)
                {
                    ordered = direction == SortDirection.DESC
                        ? query.OrderByDescending(lambda)
                        : query.OrderBy(lambda);
                }
                else
                {
                    ordered = direction == SortDirection.DESC
                        ? ordered.ThenByDescending(lambda)
                        : ordered.ThenBy(lambda);
                }
            }
            catch
            {
                continue;
            }
        }

        return ordered ?? query;
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationFilter paginationFilter)
    {
        if (paginationFilter.PageNumber <= 0)
            paginationFilter.PageNumber = 1;

        if (paginationFilter.PageSize <= 0)
            paginationFilter.PageSize = 10;

        if (paginationFilter.PageNumber > 1)
            query = query.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize);

        return query.Take(paginationFilter.PageSize);
    }
}
