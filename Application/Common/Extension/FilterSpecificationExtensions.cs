using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification;
using Application.Common.Models;
using System.Linq.Expressions;

namespace Application.Common.Extension;

public static class FilterSpecificationExtensions
{

    extension<T>(ISpecificationBuilder<T> specificationBuilder) where T : class
    {
        /// <summary>
        /// Applies SearchBy for <typeparamref name="BaseFilter"/>
        /// </summary>
        /// <param name="baseFilter"></param>
        /// <returns></returns>
        public ISpecificationBuilder<T> SearchBy(
        BaseFilter? baseFilter
        )
        {
            return specificationBuilder
                .Search(baseFilter?.AdvanceSearches)
                .Filter(baseFilter?.AdvanceFilter)
                .Sort(baseFilter?.AdvanceSort);
        }

        /// <summary>
        /// Applies search filtering to the specification builder based on the provided BaseFilter.
        /// Searches across multiple fields for the specified keyword if both are provided.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="specificationBuilder">The specification builder</param>
        /// <param name="search">The filter containing search configuration</param>
        /// <returns>The specification builder for method chaining</returns>
        public ISpecificationBuilder<T> Search(
            Search? search)
        {
            if (search is null)
                return specificationBuilder;

            if (string.IsNullOrWhiteSpace(search.Keyword) || search.Fields is null || search.Fields.Count == 0)
                return specificationBuilder;

            // Build a filter that checks if any of the specified fields contain the keyword
            var keyword = search.Keyword.Trim();

            foreach (var field in search.Fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                    continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, field);

                // Convert property to string and check if it contains the keyword (case-insensitive)
                var methodInfo = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)])
                    ?? throw new InvalidOperationException($"Could not find Contains method on string for field '{field}'");

                var propertyAsString = Expression.Call(
                    Expression.Call(property, typeof(object).GetMethod("ToString")!),
                    methodInfo,
                    Expression.Constant(keyword),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase));

                var lambda = Expression.Lambda<Func<T, bool>>(propertyAsString, parameter);
                specificationBuilder.Where(lambda);
            }

            return specificationBuilder;
        }

        /// <summary>
        /// Applies advanced filtering to the specification builder based on the provided BaseFilter.
        /// Supports nested filters with AND/OR/XOR logic and multiple comparison operators.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="specificationBuilder">The specification builder</param>
        /// <param name="baseFilter">The filter containing filter configuration</param>
        /// <returns>The specification builder for method chaining</returns>
        public ISpecificationBuilder<T> Filter(
            Filter? filter)
        {
            if (filter is null)
                return specificationBuilder;

            var parameter = Expression.Parameter(typeof(T), "x");
            var filterExpression = BuildFilterExpression<T>(filter, parameter);

            if (filterExpression is not null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                specificationBuilder.Where(lambda);
            }

            return specificationBuilder;
        }

        /// <summary>
        /// Applies sorting to the specification builder based on the provided BaseFilter.
        /// Supports multiple sort fields with ASC/DESC direction for each field.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="specificationBuilder">The specification builder</param>
        /// <param name="baseFilter">The filter containing sort configuration</param>
        /// <returns>The specification builder for method chaining</returns>
        public ISpecificationBuilder<T> Sort(
            Sort? sort)
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

                // Get the direction for this field (default to ASC if not specified)
                var direction = sort.SortDirection is not null && i < sort.SortDirection.Count
                    ? sort.SortDirection[i]
                    : Models.SortDirection.ASC;

                try
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, field);

                    // Convert to object to match the expected Expression<Func<T, object?>>
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
                    // Skip invalid field names
                    continue;
                }
            }

            return specificationBuilder;
        }
    }

    /// <summary>
    /// Recursively builds filter expressions from the filter configuration.
    /// </summary>
    private static Expression? BuildFilterExpression<T>(Filter? filter, ParameterExpression parameter)
        where T : class
    {
        if (filter is null)
            return null;

        // If this filter has nested filters, combine them with the specified logic
        if (filter.Filters is not null && filter.Filters.Any())
        {
            var expressions = filter.Filters
                .Select(f => BuildFilterExpression<T>(f, parameter))
                .OfType<Expression>()
                .ToList();

            if (expressions.Count == 0)
                return null;

            if (expressions.Count == 1)
                return expressions[0];

            var logic = (filter.Logic ?? FilterLogic.AND).ToLowerInvariant();
            return logic switch
            {
                FilterLogic.OR => CombineWithOr(expressions),
                FilterLogic.XOR => CombineWithXor(expressions),
                _ => CombineWithAnd(expressions),
            };
        }

        // If this filter has a field and operator, build a comparison expression
        if (!string.IsNullOrWhiteSpace(filter.Field) && !string.IsNullOrWhiteSpace(filter.Operator))
        {
            return BuildComparisonExpression<T>(filter.Field, filter.Operator, filter.Value, parameter);
        }

        return null;
    }

    /// <summary>
    /// Builds a comparison expression based on the field, operator, and value.
    /// </summary>
    private static Expression? BuildComparisonExpression<T>(
        string field,
        string @operator,
        object? value,
        ParameterExpression parameter)
        where T : class
    {
        try
        {
            var property = Expression.Property(parameter, field);
            var operatorLower = @operator.ToLowerInvariant();

            return operatorLower switch
            {
                FilterOperator.EQ => BuildEqualExpression(property, value),
                FilterOperator.NEQ => Expression.NotEqual(property, Expression.Constant(value)),
                FilterOperator.LT => Expression.LessThan(property, Expression.Constant(value)),
                FilterOperator.LTE => Expression.LessThanOrEqual(property, Expression.Constant(value)),
                FilterOperator.GT => Expression.GreaterThan(property, Expression.Constant(value)),
                FilterOperator.GTE => Expression.GreaterThanOrEqual(property, Expression.Constant(value)),
                FilterOperator.STARTSWITH => BuildStartsWithExpression(property, value),
                FilterOperator.ENDSWITH => BuildEndsWithExpression(property, value),
                FilterOperator.CONTAINS => BuildContainsExpression(property, value),
                _ => null,
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Builds an equality expression that handles null comparisons properly.
    /// </summary>
    private static BinaryExpression BuildEqualExpression(Expression property, object? value)
    {
        if (value is null)
            return Expression.Equal(property, Expression.Constant(null));

        return Expression.Equal(property, Expression.Constant(value));
    }

    /// <summary>
    /// Builds a StartsWith expression for string properties.
    /// </summary>
    private static MethodCallExpression? BuildStartsWithExpression(Expression property, object? value)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        var stringValue = value.ToString()!;
        var methodInfo = typeof(string).GetMethod("StartsWith", [typeof(string), typeof(StringComparison)])
            ?? throw new InvalidOperationException("Could not find StartsWith method on string");

        var propertyAsString = Expression.Call(property, typeof(object).GetMethod("ToString")!);
        return Expression.Call(
            propertyAsString,
            methodInfo,
            Expression.Constant(stringValue),
            Expression.Constant(StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Builds an EndsWith expression for string properties.
    /// </summary>
    private static MethodCallExpression? BuildEndsWithExpression(Expression property, object? value)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        var stringValue = value.ToString()!;
        var methodInfo = typeof(string).GetMethod("EndsWith", [typeof(string), typeof(StringComparison)])
            ?? throw new InvalidOperationException("Could not find EndsWith method on string");

        var propertyAsString = Expression.Call(property, typeof(object).GetMethod("ToString")!);
        return Expression.Call(
            propertyAsString,
            methodInfo,
            Expression.Constant(stringValue),
            Expression.Constant(StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Builds a Contains expression for string properties.
    /// </summary>
    private static MethodCallExpression? BuildContainsExpression(Expression property, object? value)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            return null;

        var stringValue = value.ToString()!;
        var methodInfo = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)])
            ?? throw new InvalidOperationException("Could not find Contains method on string");

        var propertyAsString = Expression.Call(property, typeof(object).GetMethod("ToString")!);
        return Expression.Call(
            propertyAsString,
            methodInfo,
            Expression.Constant(stringValue),
            Expression.Constant(StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Combines multiple expressions with AND logic.
    /// </summary>
    private static Expression CombineWithAnd(List<Expression> expressions)
    {
        return expressions.Aggregate((left, right) => Expression.AndAlso(left, right));
    }

    /// <summary>
    /// Combines multiple expressions with OR logic.
    /// </summary>
    private static Expression CombineWithOr(List<Expression> expressions)
    {
        return expressions.Aggregate((left, right) => Expression.OrElse(left, right));
    }

    /// <summary>
    /// Combines multiple expressions with XOR logic.
    /// </summary>
    private static Expression CombineWithXor(List<Expression> expressions)
    {
        // XOR: (a || b) && !(a && b)
        return expressions.Aggregate((left, right) =>
            Expression.AndAlso(
                Expression.OrElse(left, right),
                Expression.Not(Expression.AndAlso(left, right))));
    }
}
