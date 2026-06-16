using System.Linq.Expressions;
using Ardalis.Specification;
using Base.Application.Common.Models;

namespace Base.Application.Common.Extension;

public static class FilterSpecificationExtensions
{
    extension<T>(ISpecificationBuilder<T> specificationBuilder) where T : class
    {
        public ISpecificationBuilder<T> SearchBy(BaseFilter? baseFilter)
        {
            return specificationBuilder
                .Search(baseFilter?.AdvanceSearches)
                .Filter(baseFilter?.AdvanceFilter)
                .Sort(baseFilter?.AdvanceSort);
        }

        public ISpecificationBuilder<T> Search(Search? search)
        {
            if (search is null)
                return specificationBuilder;

            if (string.IsNullOrWhiteSpace(search.Keyword) || search.Fields is null || search.Fields.Count == 0)
                return specificationBuilder;

            var keyword = search.Keyword.Trim();
            var parameter = Expression.Parameter(typeof(T), "x");

            foreach (var field in search.Fields)
            {
                var fieldExpression = FilterExpressionBuilder.BuildSearchFieldExpression<T>(field, keyword, parameter);
                if (fieldExpression is null)
                    continue;

                var lambda = Expression.Lambda<Func<T, bool>>(fieldExpression, parameter);
                specificationBuilder.Where(lambda);
            }

            return specificationBuilder;
        }

        public ISpecificationBuilder<T> Filter(Filter? filter)
        {
            if (filter is null)
                return specificationBuilder;

            var parameter = Expression.Parameter(typeof(T), "x");
            var filterExpression = FilterExpressionBuilder.BuildFilterExpression<T>(filter, parameter);

            if (filterExpression is not null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                specificationBuilder.Where(lambda);
            }

            return specificationBuilder;
        }

        public ISpecificationBuilder<T> Sort(Sort? sort)
        {
            if (sort is null)
                return specificationBuilder;

            if (sort.SortBy is null || sort.SortBy.Count == 0)
                return specificationBuilder;

            for (int i = 0; i < sort.SortBy.Count; i++)
            {
                var field = sort.SortBy[i];

                if (string.IsNullOrWhiteSpace(field))
                    continue;

                var direction = sort.SortDirection is not null && i < sort.SortDirection.Count
                    ? sort.SortDirection[i]
                    : Models.SortDirection.ASC;

                try
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, field);
                    var convertedProperty = Expression.Convert(property, typeof(object));
                    var lambda = Expression.Lambda<Func<T, object?>>(convertedProperty, parameter);

                    if (direction == Models.SortDirection.DESC)
                    {
                        specificationBuilder.OrderByDescending(lambda);
                    }
                    else
                    {
                        specificationBuilder.OrderBy(lambda);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return specificationBuilder;
        }
    }
}
