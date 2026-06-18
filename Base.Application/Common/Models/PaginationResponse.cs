using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Application.Common.Models;

public interface IPagedList
{
    IReadOnlyList<object> Items { get; }
    int CurrentPage { get; }
    int TotalPages { get; }
    int TotalCount { get; }
    int PageSize { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}

public class PagedList<T>(List<T> items, int page, int count, int pageSize) : IPagedList
{
    public List<T> Items { get; set; } = items;

    IReadOnlyList<object> IPagedList.Items => Items.Cast<object>().ToList();

    public int CurrentPage { get; set; } = page;

    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);

    public int TotalCount { get; set; } = count;

    public int PageSize { get; set; } = pageSize;

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;
}

public class PaginatedResponse<T>(List<T> data, int page, int count, int pageSize)
    : PagedList<T>(data, page, count, pageSize)
{
    public List<T> Data
    {
        get => Items;
        set => Items = value;
    }
}
