using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models;

public class BaseFilter
{
    public Search? AdvanceSearches { get; set; }
    public Filter? AdvanceFilter { get; set; }
    public Sort? AdvanceSort { get; set; }

}

/// <summary>
/// Search by many field
/// </summary>
public class Search
{
    public List<string> Fields { get; set; } = [];

    public string? Keyword { get; set; }
}

/// <summary>
/// Advcance Filter
/// </summary>
public class Filter
{
    public string? Logic { get; set; }

    public IEnumerable<Filter>? Filters { get; set; }

    public string? Field { get; set; }

    public string? Operator { get; set; }

    public object? Value { get; set; }
}

public static class FilterOperator
{
    public const string EQ = "eq";
    public const string NEQ = "neq";
    public const string LT = "lt";
    public const string LTE = "lte";
    public const string GT = "gt";
    public const string GTE = "gte";
    public const string STARTSWITH = "startswith";
    public const string ENDSWITH = "endswith";
    public const string CONTAINS = "contains";
}

public static class FilterLogic
{
    public const string AND = "and";
    public const string OR = "or";
    public const string XOR = "xor";
}

public class Sort
{
    public List<string>? SortBy { get; set; }
    public List<SortDirection>? SortDirection { get; set; } // ASC or DESC
}

public enum SortDirection
{
    ASC,
    DESC
}