using System.Linq.Expressions;
using Base.Application.Common.Models;

namespace Base.Application.Common.Extension;

internal static class FilterExpressionBuilder
{
    public static Expression? BuildFilterExpression<T>(Filter? filter, ParameterExpression parameter)
        where T : class
    {
        if (filter is null)
            return null;

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

        if (!string.IsNullOrWhiteSpace(filter.Field) && !string.IsNullOrWhiteSpace(filter.Operator))
        {
            return BuildComparisonExpression(filter.Field, filter.Operator, filter.Value, parameter);
        }

        return null;
    }

    public static Expression? BuildSearchFieldExpression<T>(
        string field,
        string keyword,
        ParameterExpression parameter)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(field))
            return null;

        var property = Expression.Property(parameter, field);
        var methodInfo = typeof(string).GetMethod("Contains", [typeof(string), typeof(StringComparison)])
            ?? throw new InvalidOperationException($"Could not find Contains method on string for field '{field}'");

        var propertyAsString = Expression.Call(
            Expression.Call(property, typeof(object).GetMethod("ToString")!),
            methodInfo,
            Expression.Constant(keyword),
            Expression.Constant(StringComparison.OrdinalIgnoreCase));

        return propertyAsString;
    }

    private static Expression? BuildComparisonExpression(
        string field,
        string @operator,
        object? value,
        ParameterExpression parameter)
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

    private static BinaryExpression BuildEqualExpression(Expression property, object? value)
    {
        if (value is null)
            return Expression.Equal(property, Expression.Constant(null));

        return Expression.Equal(property, Expression.Constant(value));
    }

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

    private static Expression CombineWithAnd(List<Expression> expressions) =>
        expressions.Aggregate((left, right) => Expression.AndAlso(left, right));

    private static Expression CombineWithOr(List<Expression> expressions) =>
        expressions.Aggregate((left, right) => Expression.OrElse(left, right));

    private static Expression CombineWithXor(List<Expression> expressions) =>
        expressions.Aggregate((left, right) =>
            Expression.AndAlso(
                Expression.OrElse(left, right),
                Expression.Not(Expression.AndAlso(left, right))));
}
